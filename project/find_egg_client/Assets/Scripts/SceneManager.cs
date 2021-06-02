

using UnityEngine;

public enum GameState
{
    NONE,
    IN_MENU,
    SINGLE,
    MULTI
};

public class SceneManager : MonoBehaviour
{
    [SerializeField] private Canvas UICanvas;
    [SerializeField] private Canvas HUDCanvas;
    [SerializeField] private GameObject UI_MainMenu;
    [SerializeField] private GameObject UI_SingleMode;
    [SerializeField] private GameObject UI_MultiMode;
    [SerializeField] private LobbyManager lobby;
    [SerializeField] private GameObject gameplay;
    void SetupUI()
    {
        UI_MainMenu.SetActive(true);
        UI_SingleMode.SetActive(false);
        UI_MultiMode.SetActive(false);
    }
    void Start()
    {
        lobby.enabled = false;
        UICanvas.enabled = true;
        HUDCanvas.enabled = false;
        gameplay.SetActive(true);
        SetupUI();
    }


    void Update()
    {
       
    }
}
