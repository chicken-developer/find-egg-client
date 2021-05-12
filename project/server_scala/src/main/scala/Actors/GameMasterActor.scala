package Actors

import akka.actor.{Actor, ActorLogging}
import Core.Game._

case object GameMasterBehavior {
    case class UpdateScore(player: Player, score: String)
    case class UpdatePower(player: Player, power: String)
    case class UpdateState(player: Player, state: String)
}
class GameMasterActor extends Actor with ActorLogging {
    
    val playersInGame = collection.mutable.LinkedHashMap[String, PlayerWithActor]()

    override def receive: Receive = {

       case JoinMatch(player, actor) =>
           val newPlayer = Player(player.playerID, player.playerData);                                
           playersInGame += (newPlayer.playerID -> PlayerWithActor(newPlayer, actor))
           println(s"Player $newPlayer enter game success")
           NotifyGameUpdate()

       case LeftMatch(player) =>
           playersInGame -= player.playerID
           NotifyGameUpdate()

       case GameUpdate(player, newRequest) =>           
            log.info(s"Receive new update request is : $newRequest")
            val oldPlayerWithActor = playersInGame(player.playerID)
            val oldPlayer = oldPlayerWithActor.player

            val actor = oldPlayerWithActor.actor
            val staticData = oldPlayer.playerData.staticData

            val oldDyc = oldPlayer.playerData.dynamicData
            val newDynamicData = newRequest match {
                case s"SCORE_$newScore" =>
                    log.info(s"Update new score to : $newScore")
                    DynamicPlayerInformation(newScore.toInt, oldDyc.power, oldDyc.coin, oldDyc.isWinner)

                case s"POWER_$newPower" =>
                    log.info(s"Update new power to : $newPower")
                    DynamicPlayerInformation(oldDyc.score, newPower.toInt, oldDyc.coin, oldDyc.isWinner)

                case s"COIN_$newCoin" =>
                    log.info(s"Update new coins to : $newCoin")
                    DynamicPlayerInformation(oldDyc.score, oldDyc.power, newCoin.toInt, oldDyc.isWinner)

                case s"GAME_STATE_$newState" =>
                    log.info(s"Update new game state to : $newState")
                    DynamicPlayerInformation(oldDyc.score, oldDyc.power, oldDyc.coin, newState.toInt)

                case _ =>
                    log.info("No have any update")
                    oldDyc //No change
            }
            val newPlayerData = PlayerData(staticData, newDynamicData)
            playersInGame(player.playerID)= PlayerWithActor(Player(player.playerID, newPlayerData), actor)
            NotifyGameDynamicDataUpdate()
       case _ => log.info("Enter Game master actor")
    }

    def NotifyGameUpdate(): Unit = {
        playersInGame.values.foreach(_.actor ! GameMasterChanged(playersInGame.values.map(_.player)))
    }

    def NotifyGameDynamicDataUpdate(): Unit = {
        playersInGame.values.foreach(_.actor ! GameDynamicDataChanged(playersInGame.values.map(_.player.playerData.dynamicData)))
    }

}