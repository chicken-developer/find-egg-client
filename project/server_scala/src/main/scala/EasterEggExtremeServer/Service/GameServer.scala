package Service

import Actors.GameAreaActor
import akka.actor.{ActorRef, ActorSystem, Props}
import akka.http.scaladsl.model.ws.{Message, TextMessage}
import akka.http.scaladsl.server.Route
import akka.stream.{FlowShape, Materializer, OverflowStrategy}
import akka.stream.scaladsl.{Flow, GraphDSL, Merge, Sink, Source}
import akka.http.scaladsl.server.Directives._

import Core.Game._ 
import Actors._
class GameServer(implicit val system: ActorSystem, implicit val materializer: Materializer) {

    val gameMasterHandleActor: ActorRef = system.actorOf(Props[GameAreaActor], "GameMasterHandleActor")
    val gameMasterProfileSource: Source[GameEvent, ActorRef] = Source.actorRef[GameEvent](5,OverflowStrategy.fail)

    def gameInMatchFlow(player: Player): Flow[Message, Message, Any] =
        Flow.fromGraph(GraphDSL.create(gameMasterProfileSource) { implicit builder => profileShape =>
            import GraphDSL.Implicits._
            val materialization = builder.materializedValue.map(profileActorRef => JoinMatch(player, profileActorRef))
            val merge = builder.add(Merge[GameEvent](2))
            val gameInMatchProfileSink = Sink.actorRef[GameEvent](gameMasterHandleActor, LeftMatch(player))

            //This will tell request to actor, and actor update and push back an event
            val MessageToGameInMatchEventConverter = builder.add(Flow[Message].map {
                case TextMessage.Strict(direction) =>
                    println("Have move request from " + player.toString)
                    PositionUpdate(player, direction)

                case TextMessage.Strict(newRequest) =>
                    println("Have update data request from " + player.toString)
                    GameUpdate(player, newRequest)

            })
            //This handle back event from actor, and send text message to client
            val GameInMatchEventBackToMessageConverter = builder.add(Flow[GameEvent].map{
                case GameAreaMasterChanged(player) =>
                    import spray.json._
                    import Core.PlayerDataJsonProtocol._
                    TextMessage(player.toList.toJson.toString)

                case GameAreaDataChanged(playerData) =>
                    import spray.json._
                    import Core.PlayerDataJsonProtocol._
                    TextMessage(playerData.toList.toJson.toString)
            })

            materialization ~> merge ~> gameInMatchProfileSink
            MessageToGameInMatchEventConverter ~> merge 
            profileShape ~> GameInMatchEventBackToMessageConverter
            FlowShape(MessageToGameInMatchEventConverter.in, GameInMatchEventBackToMessageConverter.out)
        })

    import Core.Behavior._
    val GameFinalRoute: Route =
      (get & parameter("playerName") & parameter("mapPosition")) { (playerName, mapPosition) =>
        val player = Generation.GenerationPlayerData(playerName, mapPosition)
        handleWebSocketMessages(gameInMatchFlow(player))
      }

}
