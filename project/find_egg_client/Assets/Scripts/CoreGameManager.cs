
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;


public class CoreGameManager : MonoBehaviour
{
    [SerializeField] private LobbyManager lobby;
    [SerializeField] private GameObject lobbyMapObj;

    private List<PlayerDataSync> allDatas;
    private PlayerDataSync currentPlayerData;
    private static int playerIndex;

    private WebSocket coreGameWS;
    private List<string> coreGameMessages;
   
    void WaitDataFromLobby()
    {
        coreGameWS.Connect ();
        coreGameWS.OnMessage += (sender, e) =>
            {
                Debug.Log("Server akka say back " + e.Data);
                var receiveData = new string(e.Data.ToCharArray());
                coreGameMessages.Add(receiveData);
                playerIndex = coreGameMessages.Count;
            };
    }

    void InitAllData(List<string> data)
    {
        allDatas = PlayerDataSync.Init(data);
    }

    void InitGameFromLobby()
    {
        
    }
    
    void Start()
    {
        allDatas = new List<PlayerDataSync>();
        coreGameMessages = new List<string>();
        coreGameWS = new WebSocket(lobby.StartGameFromLobby());
        WaitDataFromLobby();
        Invoke("InitGameFromLobby", 0.5f);
        lobbyMapObj.SetActive(false);
        lobby.enabled = false;
    }

    void Update()
    {
        
    }
    void FixedUpdate() 
    {
        
    }
}
