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
public  class PlayerInGame
{
    public PlayerData playerData;
    public string playerIndex;
    public string playerName;
    public static List<PlayerInGame> Init(string resultFromServer)
    {
        var afterReplace = resultFromServer.Remove(0, 1).Remove(resultFromServer.Length - 2, 1).Replace("},{", "};{");
        var playerDatas = afterReplace.Split(';');
        var players = new List<PlayerInGame>();
        foreach (var playerData in playerDatas)
        {
            Debug.Log("Json: " + playerData);
            PlayerInGame player = new PlayerInGame();
            player = JsonUtility.FromJson<PlayerInGame>(playerData);
            players.Add(player);
            Debug.Log(player.playerName);
        }
        return players;
    }
}


