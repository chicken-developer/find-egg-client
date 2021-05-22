
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using Debug = UnityEngine.Debug;

public enum GameState
{
    IN_LOBBY,
    IN_INGAME,
}

public class SceneManager : MonoBehaviour
{
    //UI
    [SerializeField] private InputField playerNameField;
    [SerializeField] private Button startGameBtn;
    [SerializeField] private Text waitingTextAlert;
    [SerializeField] private Canvas UICanvas;
    
    //Network
    private string _server_address;
    private WebSocket _client;
    private string receiveData0 = "";
    private string receiveData1 = "";

    private int _currentPlayerInMatch;
    private int _maxPlayerInMatch;
    private GameState _gameState;
    [SerializeField] private List<GameObject> opponents;
    
    //Network Behaviors
    private void SendData(string message, WebSocket ws)
    {
        ws.Send(message);
        ws.OnMessage += (sender, e) => {
            Debug.Log("Message received from " + ((WebSocket)sender).Url + " is: " + e.Data);
        };
    }
    private string Ask0(string message, WebSocket ws)
    {
        ws.Connect();
        ws.Send(message);
        ws.OnMessage += (sender, e) => {
            Debug.Log("EData  0 : " + e.Data);
            receiveData0 = e.Data;
        };
        Debug.Log("Receive ASK 0: " + receiveData0);
        return receiveData0;
    }
    private string Ask1(string message, WebSocket ws)
    {
        ws.Connect();
        ws.Send(message);
        ws.OnMessage += (sender, e) => {
            Debug.Log("EData  1 : " + e.Data);
            receiveData1 = e.Data;
        };
        Debug.Log("Receive ASK 1 : " + receiveData1);
        return receiveData1;
    }
    //Network Behaviors______End.

    
    //Game Event
    private void OnClickStartGameBtn()
    {
        var playerName = playerNameField.text;
        _server_address = "wss://localhost:8005";
        var mapPosition = new Vector2(10, 10).ToString();
        Debug.Log("Map position: " + mapPosition);
        var finalAddress = _server_address + "/?playerName=" + playerName + "&mapPosition=" + mapPosition;
        _client = new WebSocket(finalAddress);
        _client.Connect();
        
        EnterLobby();
    }
    private void EnterLobby()
    {
        _gameState = GameState.IN_LOBBY;
        playerNameField.gameObject.SetActive(false);
        startGameBtn.gameObject.SetActive(false);
        waitingTextAlert.enabled = true;
        string maxPlayer = Ask0("SPECIAL_REQUEST_MAX_PLAYER", _client);
        string currentPlayer = Ask1("SPECIAL_REQUEST_CURRENT_PLAYER", _client);
        waitingTextAlert.text += "\n Current player in game: " + currentPlayer + " / " + maxPlayer;
    }
    //Game Event______End.

   
    void MovementUpdate()
    {
        
    }
    void EggScoreUpdate()
    {
        
    }
    void LobbyUpdate()
    {
        if (_gameState != GameState.IN_LOBBY) return;
        if (_client.IsAlive)
        {
           
        }
    }

    void InGameUpdate()
    {
        if(_gameState != GameState.IN_INGAME) return;
        if (_client.IsAlive)
        {
            MovementUpdate();
            EggScoreUpdate();
        }
    }
    
    void Start()
    {
        waitingTextAlert.enabled = false;
        startGameBtn.onClick.AddListener(OnClickStartGameBtn);
    }

    void Update()
    {
        switch (_gameState)
        {
            case GameState.IN_LOBBY:
                LobbyUpdate();
                break;
            case GameState.IN_INGAME:
                InGameUpdate();
                break;
        }
    }
}
