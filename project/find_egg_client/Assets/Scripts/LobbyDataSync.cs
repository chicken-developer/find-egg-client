/*
 *
 * [{"playerIndex":4,"playerName":"asdasd","position":{"x":0.0,"y":0.0},"serverWillJoin":"ws://192.168.1.9/8088/?playerName=demo&mapPosition=20_00"},
 * {"playerIndex":3,"playerName":"demo","position":{"x":0.0,"y":0.0},"serverWillJoin":"ws://192.168.1.9/8088/?playerName=demo&mapPosition=20_00"}]
 */

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LobbyPosition
{
 public float x;
 public float y;
}

[Serializable]
public  class LobbyDataSync
{
 public string playerIndex;
 public string playerName;
 public Position position;
 public string serverWillJoin;
 
 public static List<LobbyDataSync> Init(string input)
 {
  var playersInLobby = new List<LobbyDataSync>();
  var afterReplace = input.Remove(0, 1).Remove(input.Length - 2, 1).Replace("},{", "};{");
  var playersData = afterReplace.Split(';');
   foreach (var playerData in playersData)
   {
    Debug.Log("Json: " + playerData);
    LobbyDataSync playerInLobby = new LobbyDataSync();
    playerInLobby = JsonUtility.FromJson<LobbyDataSync>(playerData);
    playersInLobby.Add(playerInLobby);
    Debug.Log(playerInLobby.playerName);
   }
   return playersInLobby;
 }
}

