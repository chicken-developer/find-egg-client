using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private PlayerDataSync controlPlayerData;
    private List<PlayerDataSync> allPlayerData;

    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> opponent;

    void MovementUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            Vector3 position = player.transform.position;
            position.x--;
            this.transform.position = position;

        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Vector3 position = player.transform.position;
            position.x++;
            this.transform.position = position;

           
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Vector3 position = player.transform.position;
            position.y++;
            this.transform.position = position;

        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            Vector3 position = player.transform.position;
            position.y--;
            this.transform.position = position;

        }
    }
    
    void SyncDataWithServer(PlayerDataSync newData)
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovementUpdate();
    }
}
