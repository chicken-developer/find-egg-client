package Core 
import akka.actor.ActorRef

object Game {
    trait GameData
        case class PlayerWithActor(player: Player, actor: ActorRef) extends GameData
        case class Player(playerName: String, currentEgg: Int, position: Position, currentState: Int) extends GameData
        case class Position(x:Int, y:Int) extends GameData {
            def + (other: Position) : Position = {
                Position(x+other.x, y+other.y)
            }
        }

    trait GameEvent
        case class EnterGameMaster(player: Iterable[PlayerWithActor]) extends GameEvent
        case class GameUpdate(player: Player, newData: String) extends GameEvent
        case class GameMasterChanged(player: Iterable[Player]) extends GameEvent
        case class JoinMatch(player: Player, actor: ActorRef) extends GameEvent
        case class LeftMatch(player: Player) extends GameEvent

}

import spray.json._
object PlayerDataJsonProtocol extends DefaultJsonProtocol {
    import Core.Game._
    implicit val dynamicInfoFormat = jsonFormat4(DynamicPlayerInformation)
    implicit val staticInfoFormat = jsonFormat3(StaticPlayerInformation)
    implicit val playerDataFormat = jsonFormat2(PlayerData)
    implicit val PlayerFormat = jsonFormat2(Player)

}

