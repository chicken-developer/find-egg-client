package Service

import Actors.GameMasterActor
import akka.actor.{ActorRef, ActorSystem, Props}
import akka.http.scaladsl.model.ws.{Message, TextMessage}
import akka.http.scaladsl.server.Route
import akka.stream.{FlowShape, Materializer, OverflowStrategy}
import akka.stream.scaladsl.{Flow, GraphDSL, Merge, Sink, Source}
import akka.http.scaladsl.server.Directives._
import akka.http.scaladsl.Http
import akka.http.scaladsl.model.{ContentTypes, HttpEntity, StatusCodes}
import akka.stream.ActorMaterializer
import Core.Game._ 
import Actors._
class GameServer(implicit val system: ActorSystem, implicit val materializer: Materializer) {

    val gameMasterHandleActor: ActorRef = system.actorOf(Props[GameMasterActor], "GameMasterHandleActor")
    val gameMasterProfileSource: Source[GameEvent, ActorRef] = Source.actorRef[GameEvent](5,OverflowStrategy.fail)

    def gameInMatchFlow(player: Player): Flow[Message, Message, Any] =
        Flow.fromGraph(GraphDSL.create(gameMasterProfileSource) { implicit builder => profileShape =>
            import GraphDSL.Implicits._
            val materialization = builder.materializedValue.map(profileActorRef => JoinMatch(player, profileActorRef))
            val merge = builder.add(Merge[GameEvent](2))
            val gameInMatchProfileSink = Sink.actorRef[GameEvent](gameMasterHandleActor, LeftMatch(player))

            //This will tell request to actor, and actor update and push back an event
            val MessageToGameInMatchEventConverter = builder.add(Flow[Message].map {
                case TextMessage.Strict(newRequest) =>
                    println("Have update data request from " + player.toString)
                    GameUpdate(player, newRequest)

            })
            //This handle back event from actor, and send text message to client
            val GameInMatchEventBackToMessageConverter = builder.add(Flow[GameEvent].map{
                case GameMasterChanged(player) =>
                    import spray.json._
                    import Core.PlayerDataJsonProtocol._
                    TextMessage(player.toList.toJson.toString)
                    
                case GameDynamicDataChanged(playerDynamicData) =>
                    import spray.json._
                    import Core.PlayerDataJsonProtocol._
                    TextMessage(playerDynamicData.toList.toJson.toString)
            })

            materialization ~> merge ~> gameInMatchProfileSink
            MessageToGameInMatchEventConverter ~> merge 
            profileShape ~> GameInMatchEventBackToMessageConverter
            FlowShape(MessageToGameInMatchEventConverter.in, GameInMatchEventBackToMessageConverter.out)
        })


    val GameFinalRoute: Route =
      (get & parameter("playerID") & parameter("playerName") & parameter("dragonID") & parameter("backgroundID"))
      {(playerID, playerName, dragonID, backgroundID) =>
                val playerData = PlayerData(StaticPlayerInformation(playerName, dragonID, backgroundID),
                                            DynamicPlayerInformation(0, 0, 0, 0))
                handleWebSocketMessages(gameInMatchFlow(Player(playerID, playerData)))
      }

}
