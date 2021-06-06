using System;

public  class PlayerDataLocal
{
    public static string playerUserName;
    
    public static void Init(string data)
    {
        var playerDatas = data.Split(';');
        var playerDataLocal = new PlayerDataLocal();
        playerUserName = playerDatas[0];
    }
    public static string GenerationDataForLobby()
    {
        return playerUserName + "-0025_0000"; //TODO: Generation location if need
    }
}