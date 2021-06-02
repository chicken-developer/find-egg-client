using UnityEngine;
using WebSocketSharp;

namespace Networks.NetworkBehaviors
{
    public class LobbyBehavior: NetworkBehavior
    {
        private static LobbyBehavior _instance;
        private string serverAddress = "ws://103.153.65.194:8086/echo";
        public static LobbyBehavior GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LobbyBehavior();
            }
            return _instance;
        }

        public bool JoinLobby(Player player)
        {
            using (var ws = new WebSocket (serverAddress)) {
                ws.OnMessage += (sender, e) =>
                  Debug.Log("Server say back " + e.Data);

                ws.Connect ();
                ws.Send ("Hello cpp");
            }
            return true;
        }
    }
}