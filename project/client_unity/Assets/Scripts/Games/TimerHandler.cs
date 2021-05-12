using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{
    [SerializeField] private int _minute;
    [SerializeField] private int _seconds;
    private float coolDownTimer;
    
    void TimerUpdate()
    {
        if (coolDownTimer > 0)
        {
            coolDownTimer -= Time.deltaTime;
        }

        if (coolDownTimer < 0)
        {
            coolDownTimer = 0;
        }

        TimerUIUpdate(coolDownTimer);
    }

    void TimerUIInit(int minus, int second)
    {
        
    }
    void TimerUIUpdate(float timer)
    {
        
    }
    void Start()
    {
        //Init timer
        coolDownTimer = _minute * 60 + _seconds;
        TimerUIInit(_minute, _seconds);
    }

    void Update()
    {
        TimerUpdate();
    }
}
