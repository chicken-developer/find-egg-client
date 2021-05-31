using UnityEngine;
using WebSocketSharp;

namespace Networks.NetworkBehaviors
{
    public class CoreGameBehavior: NetworkBehavior
    {
        private static CoreGameBehavior _instance;
        private string serverAddress = "wss://192.168.220.129:8085/?playerName=playerDemo&mapPosition=101010";
        public static CoreGameBehavior GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CoreGameBehavior();
            }
            return _instance;
        }

        public bool EnterGame(Player player)
        {
            using (var ws = new WebSocket (serverAddress)) {
                ws.OnMessage += (sender, e) =>
                    Debug.Log("Server akka say back " + e.Data);

                ws.Connect ();
                ws.Send ("Hello akka");
            }
            return true;
        }
    }
}