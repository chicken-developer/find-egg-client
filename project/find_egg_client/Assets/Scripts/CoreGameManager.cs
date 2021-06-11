
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

    void SyncAllDataWithServer()
    {
        allDatas = PlayerDataSync.Init(coreGameMessages[coreGameMessages.Count - 1]);
        currentPlayerData = allDatas.Find(player => player.playerName == PlayerDataLocal.playerUserName);
    }
    void move()
    {
        Vector3 newPosition = player.transform.position;
        newPosition.x = currentPlayerData.playerData.position.x ;
        newPosition.y = currentPlayerData.playerData.position.y;
        player.transform.position = newPosition;
    }
    void MovementUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            coreGameWS.Send("MOVE_REQUEST:left");
            move();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            coreGameWS.Send("MOVE_REQUEST:right");
            move();
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            coreGameWS.Send("MOVE_REQUEST:up");
            move();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        { 
            coreGameWS.Send("MOVE_REQUEST:down");
            move();
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
        if (coreGameMessages.Count > 1000)
        {
            coreGameMessages.RemoveRange(0,500);
        }

        SyncAllDataWithServer();
    }
}
