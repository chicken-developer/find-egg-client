
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] GameObject player;
    [SerializeField] private List<GameObject> opponents;
    [SerializeField] private string playerName;
    private string _server_address;
    private WebSocket _client;
    private string receiveData;
    private GameState _game_state;
    void InitDataForOnlineMode()
    {
        Debug.Log("Init profile for multi mode");

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


    private void ReConnect()
    {
        //TODO: Reconnect to server   
    }
    void Start()
    {
        _server_address = "wss://localhost:8005";
        var mapPosition = new Vector2(10, 10).ToString();
        Debug.Log("Map position: " + mapPosition);
        var finalAddress = _server_address + "/?playerName=" + playerName + "&mapPosition=" + mapPosition;
        _client = new WebSocket(finalAddress);
        _client.Connect();
        
        if(_client.IsAlive)
        {
            InitDataForOnlineMode();
        }

    }

    void Update()
    {
        MovementUpdate();
        EggScoreUpdate();
    }
}
