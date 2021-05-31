using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsPlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject enemy;
    void Start()
    {
        
    }
    void MovementUdpate()
    {
         if (Input.GetKeyDown(KeyCode.LeftArrow))
                 {
                         Vector3 position = this.transform.position;
                         position.x--;
                         this.transform.position = position;

                        Vector3 enemyPos = enemy.transform.position;
                        enemyPos.x++;
                        enemy.transform.position = enemyPos;

                 }
                 if (Input.GetKeyDown(KeyCode.RightArrow))
                 {
                    Vector3 position = this.transform.position;
                    position.x++;
                    this.transform.position = position;

                    Vector3 enemyPos = enemy.transform.position;
                    enemyPos.x--;
                    enemy.transform.position = enemyPos;
                 }
                 if (Input.GetKeyDown(KeyCode.UpArrow))
                 {
                         Vector3 position = this.transform.position;
                         position.y++;
                         this.transform.position = position;

                          Vector3 enemyPos = enemy.transform.position;
                    enemyPos.y--;
                    enemy.transform.position = enemyPos;
                 }
                 if (Input.GetKeyDown(KeyCode.DownArrow))
                 {
                         Vector3 position = this.transform.position;
                         position.y--;
                         this.transform.position = position;

                          Vector3 enemyPos = enemy.transform.position;
                    enemyPos.y++;
                    enemy.transform.position = enemyPos;
                 }
    }
    // Update is called once per frame
    void Update()
    {
        MovementUdpate();
    }
}
