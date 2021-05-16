
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public enum GameState
{
    IN_SINGLE,
    IN_MULTI,
    IN_MAINMENU,
    IN_PAUSE,
    IN_WINDOWS // Win game, wait game,...
}

public class SceneManager : MonoBehaviour
{
    //UI
    [SerializeField] private InputField playerNameField;
    [SerializeField] private Button startGameBtn;
    [SerializeField] private Canvas UICanvas;
    
    //Network
    private string _server_address;
    private WebSocket _client;
    private string receiveData;
    
    
    [SerializeField] private List<GameObject> opponents;
    private GameState _game_state;
    
    //Network Behaviors
    private void SendData(string message, WebSocket ws)
    {
        ws.Send(message);
        ws.OnMessage += (sender, e) => {
            Debug.Log("Message received from " + ((WebSocket)sender).Url + " is: " + e.Data);
        };
    }

    private string Ask(string message, WebSocket ws)
    {
        ws.Connect();
        ws.Send(message);
        string data = "NULL";
        ws.OnMessage += (sender, e) => {
            receiveData = e.Data;
            Debug.Log("Receive ASK: " + receiveData);
        };

        return receiveData;
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
        playerNameField.enabled = false;
        EnterLobby();
    }

    private void EnterLobby()
    {
        if (_client.IsAlive && opponents.Count > 1)
        {
            playerNameField.enabled = false;
        }
    }
    //Game Event______End.
    
    //Call on Start()
    void InitDataForOnlineMode()
    {
        Debug.Log("Init profile for multi mode");

    }

    
    //Call on Update()
    void LobbyUpdate()
    {
        Debug.Log("Waiting another player");
    }
    void MovementUpdate()
    {
        if (_game_state is GameState.IN_MULTI)
        {
            
        }
    }
    void EggScoreUpdate()
    {
        if (_game_state is GameState.IN_MULTI)
        {
            
        }
    }
    
    
    void Start()
    {
        startGameBtn.onClick.AddListener(OnClickStartGameBtn);
        if(_client.IsAlive)
        {
            InitDataForOnlineMode();
        }

    }

    void Update()
    {
        LobbyUpdate();
        MovementUpdate();
        EggScoreUpdate();
    }
}
