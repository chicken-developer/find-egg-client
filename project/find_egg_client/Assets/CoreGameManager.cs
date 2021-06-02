
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;


public class CoreGameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> enemy;
    private string serverLocalAddress = "ws://192.168.220.129:8089/?playerName=vothaocute&mapPosition=2_4";
    private string serverAddress = "ws://103.153.65.194:8089/?playerName=quynhcute&mapPosition=hellomap";
    private string receiveData;
    private WebSocket coreWS;
    private List<PlayerInGame> players ;
    private InGameState state;
    float timeToGo;

    enum InGameState
    {
        INIT,
        IN_GAME,
    }
    //Note that name and server must get from lobby and send to c++ server
    void GetDataFromLobby()
    {
        coreWS.Connect ();
        coreWS.OnMessage += (sender, e) =>
            {
                Debug.Log("Server akka say back " + e.Data);
                receiveData = e.Data;
                coreWS.Close();
            };
        
    }

    void InitPlayerFromLobby()
    {
        players = PlayerInGame.Init(receiveData);
        Debug.Log("Player 0 name " + players[0].playerName);
        var controlPlayer = players[0];
        //player.transform.position = new Vector3(controlPlayer.playerData.position.x,controlPlayer.playerData.position.x, player.transform.position.z);
        player.transform.position =  new Vector3(0, 0, 0);
        Debug.Log("Init data completed");
    }
    void Start()
    {
        players = new List<PlayerInGame>();
        state = InGameState.INIT;
        coreWS = new WebSocket(serverLocalAddress);
        GetDataFromLobby();
        timeToGo = Time.fixedTime + 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void FixedUpdate() {
        if (Time.fixedTime >= timeToGo) {
            timeToGo = Time.fixedTime + 60.0f;
            InitPlayerFromLobby();
            Debug.Log("Enter fix update");
        }
    }
}
