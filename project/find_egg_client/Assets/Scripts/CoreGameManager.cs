
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebSocketSharp;

public class CoreGameManager : MonoBehaviour
{
    [SerializeField] private GameObject lobbyPrefab;
    
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<GameObject> enemies;
    private List<PlayerDataSync> enemiesDatas;
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
        enemiesDatas = allDatas.FindAll(player => player.playerName != PlayerDataLocal.playerUserName);
        enemies = new List<GameObject>(enemiesDatas.Count);
        
        foreach (var enemyData in enemiesDatas)
        {
            var enemy = new GameObject();
            enemy = Instantiate(enemyPrefab, new Vector3(enemyData.playerData.position.x, enemyData.playerData.position.y, 0), Quaternion.identity);
            enemies.Add(enemy);
        }
        Debug.Log("Now have " + allDatas.Count + "  data in game");
        Debug.Log("Now have " + enemiesDatas.Count + " enemy data in game");
        Debug.Log("Now have " + enemies.Count + " enemy object in game");

    }

    void SyncAllDataWithServer()
    {
        allDatas = PlayerDataSync.Init(coreGameMessages[coreGameMessages.Count - 1]);
        currentPlayerData = allDatas.Find(player => player.playerName == PlayerDataLocal.playerUserName);
        enemiesDatas = allDatas.FindAll(enemy => enemy.playerName != PlayerDataLocal.playerUserName);
        Debug.Log("Sync have " + allDatas.Count + "  data in game");
        Debug.Log("Sync have " + enemiesDatas.Count + " enemy data in game");
        Debug.Log("Sync have " + enemies.Count + " enemy object in game");
        SyncEnemyData();
    }

    void SyncEnemyData()
    {
        foreach(var data in  enemiesDatas.Zip(enemies, Tuple.Create))
        {
            Vector3 newPosition = data.Item2.transform.position;
            newPosition.x = data.Item1.playerData.position.x ;
            newPosition.y = data.Item1.playerData.position.y;
            data.Item2.transform.position = newPosition;
        }

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
        coreGameWS = new WebSocket(lobbyPrefab.GetComponent<LobbyManager>().StartGameFromLobby());
        Debug.Log("Current lobby url is: " + lobbyPrefab.GetComponent<LobbyManager>().StartGameFromLobby());
        WaitDataFromLobby();
        lobbyPrefab.SetActive(false);
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
