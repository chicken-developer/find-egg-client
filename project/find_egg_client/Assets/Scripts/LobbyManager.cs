using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private CoreGameManager coreGame;
    [SerializeField] private GameObject coreGameObj;
    [SerializeField] private Text title;
    private WebSocket lobbyWS;
    private bool isGameHaveStarted;
    private int lobbySize = 3;
    private List<string> lobbyMessages;
    private List<PlayerDataSync> playersDataFromLobby;
    private static int playerIndex;
    public void JoinOrQuitLobby(string playerData = "null") // This will call from another scene to put player into lobby
    {
        lobbyWS.Connect ();
        lobbyWS.OnMessage += (sender, e) =>
        {
            Debug.Log("Receive new data: " + e.Data);
            var receiveData = new string(e.Data.ToCharArray());
            lobbyMessages.Add(receiveData);
            Debug.Log("Current message size: " + lobbyMessages.Count);
            Debug.Log("Newest message: " + lobbyMessages[lobbyMessages.Count]);

            playerIndex = lobbyMessages.Count;
            Debug.Log("Current player index: " + playerIndex);

        };
        //Sent playerName and position only: playerName=asdasd&mapPosition=1_2
        lobbyWS.Send(playerData); // This server will return an address for player join match
        //EX: ws:192.168.220.129:8089/?playerName=asdasd&mapPosition=1_2
    }

    public string StartGameFromLobby()
    {
        coreGameObj.SetActive(true);
        coreGame.enabled = true;
        isGameHaveStarted = true;
        Debug.Log("Current player index is: " + playerIndex);
        Debug.Log("Current player in game is: " + lobbyMessages.Count);
        return lobbyMessages[playerIndex]; //ws:192.168.220.129:8089/?playerName=thaocute&mapPosition=1_2
    }
    void LobbyUpdate()
    {
        Debug.Log("Lobby still update");
        title.text = lobbyMessages.Count + " / " + lobbySize +" players join to lobby" + "\n You is player " + playerIndex;
        if (lobbyMessages.Count >= lobbySize)
        {
            title.text = "READY TO START GAME BABIES..";
            Invoke("StartGameFromLobby", 4.0f);
        }
    }
    
    void Start()
    {
        coreGame.enabled = false;
        coreGameObj.SetActive(false);
        isGameHaveStarted = false;
        lobbyMessages = new List<string>();
        lobbyWS = new WebSocket("ws://192.168.220.129:8086/privateLobby");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isGameHaveStarted)
            LobbyUpdate();
        
    }
}
