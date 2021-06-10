using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Position
{
    public float x;
    public float y;
}

[Serializable]
public class PlayerData
{
    public int currentPoint;
    public Position eggPosition;
    public Position position;
}

[Serializable]
public  class PlayerDataSync
{
    public PlayerData playerData;
    public string playerIndex;
    public string playerName;
    public static List<PlayerDataSync> Init(string resultFromServer)
    {
        var players = new List<PlayerDataSync>();
          var afterReplace = resultFromServer.Remove(0, 1).Remove(resultFromServer.Length - 2, 1).Replace("},{", "};{");
            var playerDatas = afterReplace.Split(';');
            foreach (var playerData in playerDatas)
            {
                Debug.Log("Json: " + playerData);
                PlayerDataSync player = new PlayerDataSync();
                player = JsonUtility.FromJson<PlayerDataSync>(playerData);
                players.Add(player);
                Debug.Log(player.playerName);
            }
        return players;
    }
}


