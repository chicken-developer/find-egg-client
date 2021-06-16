using System;

public  class PlayerDataLocal
{
    public static string playerUserName;
    
    public static void Init(string data)
    {
        //TODO: Not use data now
        var playerDatas = "demo;-22_00".Split(';');
        var playerDataLocal = new PlayerDataLocal();
        playerUserName = playerDatas[0];
    }
    public static void Update(string newData)
    {
        var playerDatas = newData.Split(';');
        var playerDataLocal = new PlayerDataLocal();
        playerUserName = playerDatas[0];
    }
    public static string GenerationDataForLobby()
    {
        return playerUserName + "-0025_0000"; //TODO: Generation location if need
    }
}