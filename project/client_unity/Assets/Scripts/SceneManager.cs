
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using Debug = UnityEngine.Debug;

public enum GameState
{
    NONE,
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
    private List<string> staticData;
    private List<string> dynamicData;

    private int _currentPlayerInMatch;
    private int _maxPlayerInMatch;
    private GameState _gameState;
    [SerializeField] private List<GameObject> opponents;
  
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
        playerNameField.gameObject.SetActive(false);
        startGameBtn.gameObject.SetActive(false);
        _client.Connect();
        _client.Send("SPECIAL_REQUEST_CURRENT_PLAYER");
        _client.Send("SPECIAL_REQUEST_MAX_PLAYER");
        waitingTextAlert.enabled = true;
        _gameState = GameState.IN_LOBBY;
    }

   
    void MovementUpdate()
    {
        
    }
    void EggScoreUpdate()
    {
        
    }

    void NetworkUpdate()
    {
        
    }
    void LobbyUpdate()
    {
        if (_gameState != GameState.IN_LOBBY) return;
        var currentPlayer = System.Convert.ToInt32(staticData[1]);
        var maxPlayer = System.Convert.ToInt32(staticData[2]);
        if (_client.IsAlive)
        {
            waitingTextAlert.text = "Waiting another player join game... \n Current player in joining are: " +currentPlayer + " / " + maxPlayer;
            if (currentPlayer < maxPlayer)
            {
                _gameState = GameState.IN_INGAME;
            }
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
        _gameState = GameState.NONE;
        staticData = new List<string>();
        dynamicData = new List<string>();
        waitingTextAlert.enabled = false;
        startGameBtn.onClick.AddListener(OnClickStartGameBtn);
    }


    void Update()
    {
        NetworkUpdate();
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
