
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebSocketSharp;


public class CoreGameManager : MonoBehaviour
{
    [SerializeField] private LobbyManager lobby;
    [SerializeField] private GameObject lobbyMapObj;
    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> enemies;
    private List<PlayerDataSync> allDatas;
    private PlayerDataSync currentPlayerData;
    
    private WebSocket coreGameWS;
    private List<string> coreGameMessages;
    private string playerName;
    void WaitDataFromLobby()
    {
        coreGameWS.Connect ();
        coreGameWS.OnMessage += (sender, e) =>
            {
                Debug.Log("Receive new data: " + e.Data);
                var receiveData = new string(e.Data.ToCharArray());
                coreGameMessages.Add(receiveData);
            };
    }


    void InitDataFromLobby()
    {
        allDatas = PlayerDataSync.Init(coreGameMessages[coreGameMessages.Count - 1]);
        currentPlayerData = allDatas.Find(player => player.playerName == PlayerDataLocal.playerUserName);
        player.transform.position = new Vector3(currentPlayerData.playerData.position.x,currentPlayerData.playerData.position.y, player.transform.position.z );
        //TODO: Init for enemies;
    }
    void SyncDataWithServer(string serverData)
    {
        currentPlayerData = allDatas.Find(player => player.playerName == PlayerDataLocal.playerUserName);
        Debug.Log("Found current player is: " + currentPlayerData.playerName);
        SyncMovement(currentPlayerData);
    }
   
    void SyncMovement(PlayerDataSync syncData)
    {
        Debug.Log("Enter sync movement with new x and y:" + syncData.playerData.position.x + " : " + syncData.playerData.position.y);
        Vector3 position = player.transform.position;
        var newX = syncData.playerData.position.x;
        var newY = syncData.playerData.position.y;
        Debug.Log("+= x is: " + (newX - position.x) + " and += y is" + (newY - position.y));
        position.x += (newX - position.x) ;
        position.y += (newY - position.y);
        player.transform.position = position;
    }
    
    void MovementUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            coreGameWS.Send("MOVE_REQUEST:left");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            coreGameWS.Send("MOVE_REQUEST:right");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            coreGameWS.Send("MOVE_REQUEST:up");

        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        { 
            coreGameWS.Send("MOVE_REQUEST:down");
        }
        
    }
    
    void Start()
    {
        allDatas = new List<PlayerDataSync>();
        coreGameMessages = new List<string>();
        coreGameWS = new WebSocket(lobby.StartGameFromLobby());
        Debug.Log("Current lobby url is: " + lobby.StartGameFromLobby());
        WaitDataFromLobby();
        lobbyMapObj.SetActive(false);
        lobby.enabled = false;
        Invoke("InitDataFromLobby", 0.5f);
    }

    void Update()
    {
        MovementUpdate();
    }
    void FixedUpdate() 
    {
        
    }
}
