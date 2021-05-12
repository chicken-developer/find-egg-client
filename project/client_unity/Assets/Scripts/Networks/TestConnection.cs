using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
public class TestConnection : MonoBehaviour
{
    void Start()
    {
         using (var ws = new WebSocket ("wss://localhost:8005?playerName=NguyenQuynhxczxcz")) {
                        ws.OnMessage += (sender, e) =>
                            Debug.Log("Laputa says: " + e.Data);
        
                        ws.Connect ();
                        ws.Send ("BALUS");
                    }
    }

    void Update()
    {
        
    }
}
