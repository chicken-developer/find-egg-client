
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;


public class CoreGameManager : MonoBehaviour
{
    private WebSocket coreGameWS;
    private List<string> coreGameMessages;
   
    void GetDataFromLobby()
    {
        coreGameWS.Connect ();
        coreGameWS.OnMessage += (sender, e) =>
            {
                Debug.Log("Server akka say back " + e.Data);
                var receiveData = new string(e.Data.ToCharArray());
                coreGameMessages.Add(receiveData);
            };
    }
    
    void StartGameFromLobby()
    {
        Debug.Log("Current message receive: " + coreGameMessages.Count);
        Debug.Log("First message: " + coreGameMessages[0]);
    }
    
    void Start()
    {
        coreGameMessages = new List<string>();
        coreGameWS = new WebSocket("ws://192.168.220.129:8089/?playerName=thaocute&mapPosition=1_1");
        GetDataFromLobby();
        Invoke("StartGameFromLobby", 0.5f);
    }

    void Update()
    {
       
    }
    void FixedUpdate() 
    {
      
    }
}
