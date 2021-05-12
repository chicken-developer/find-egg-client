
using UnityEngine;
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
    
    
    [SerializeField] EggHandler eggHandler;
    [SerializeField] TimerHandler timerHandler;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
