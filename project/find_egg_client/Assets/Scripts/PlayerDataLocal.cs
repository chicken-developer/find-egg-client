using System;

public  class PlayerDataLocal
{
    public static string playerUserName;
    public PlayerDataSync playerSyncData;
    
    public static PlayerDataLocal Init(string data)
    {
        var playerDatas = data.Split(';');
        var playerDataLocal = new PlayerDataLocal();
        playerUserName = playerDatas[0];
        playerDataLocal.playerSyncData = new PlayerDataSync();
        return playerDataLocal;
    }
    public static string GenerationDataForLobby()
    {
        return playerUserName + "-25_0"; //TODO: Generation location if need
    }
}