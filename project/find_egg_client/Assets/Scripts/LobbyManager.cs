using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class LobbyManager : MonoBehaviour
{
    private string playerName;
    private string mapPosition;
    private string serverLocalAddress;
    private WebSocket lobbyWS;

    private int lobbySize = 120;
    private List<string> lobbyMessages;
    private List<PlayerDataSync> playersDataFromLobby;
    
    public void JoinOrQuitLobby(string playerData = "null") // This will call from another scene to put player into lobby
    {
        lobbyWS.Connect ();
        lobbyWS.OnMessage += (sender, e) =>
        {
            var receiveData = new string(e.Data.ToCharArray());
            lobbyMessages.Add(receiveData);
        };
        lobbyWS.Send(playerData); // This server will return an address for player join match
        //EX: ws:192.168.220.129:8089/?playerName=asdasd&mapPosition=1_2
    }

    void HandleReceive(List<string> lobbyMessages)
    {
        //
    }
    void LobbyUpdate()
    {
        Debug.Log("Current message receive: " + lobbyMessages.Count);
        HandleReceive(lobbyMessages);
    }
   
    
    void Start()
    {
        lobbyMessages = new List<string>();
        lobbyWS = new WebSocket("ws://192.168.220.129:8086/publicLobby");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        LobbyUpdate();
    }
}
