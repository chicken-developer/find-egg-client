using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private string _server_address;
    private NetworkBehavior _client;
    void InitDataForOnlineMode()
    {
    }

    void InitDataForOfflineMode()
    {
    }
    void MovementUpdate(GameState gameState)
    {
        if (gameState is GameState.IN_SINGLE)
        {
            //TODO: Handle input movement 
        }

        if (gameState is GameState.IN_MULTI)
        {
            
        }
    }

    void EggScoreUpdate(GameState gameState)
    {
        if (gameState is GameState.IN_SINGLE)
        {
            //TODO: Handle input movement 
        }

        if (gameState is GameState.IN_MULTI)
        {
            
        }
    }
    void Start()
    {
        _server_address = "wss://localhost:8005";
        _client = new NetworkBehavior();
        
        if(_client.Connect(_server_address))
        {
            InitDataForOnlineMode();
        }
        else
        {
            InitDataForOfflineMode();
        }
        
        
    }

    void Update(GameState gameState)
    {
        MovementUpdate(gameState);
        EggScoreUpdate(gameState);
    }
}
