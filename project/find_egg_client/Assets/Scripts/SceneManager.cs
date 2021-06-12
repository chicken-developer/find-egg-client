

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameObject UI_MainMenu;
    [SerializeField] private GameObject UI_SingleMode;
    [SerializeField] private GameObject UI_MultiMode;
    
    [SerializeField] private Button ui_btn_SingleMode;
    [SerializeField] private Button ui_btn_MultiMode;
    [SerializeField] private Button multiMode_btn_Login;
    [SerializeField] private InputField multiMode_if_UserName;
    [SerializeField] private InputField multiMode_if_Password;


    [SerializeField] private CoreGameManager inGameMenu;
    [SerializeField] private GameObject inGameMenuPrefab;
    [SerializeField] private GameObject gameplayPrefab;
    [SerializeField] private GameObject lobbyPrefab;

    void SetupUI()
    {
        UI_MainMenu.SetActive(true);
        UI_SingleMode.SetActive(false);
        UI_MultiMode.SetActive(false);
    }
    void Start()
    {
        inGameMenuPrefab.SetActive(false);
        gameplayPrefab.SetActive(false);
        lobbyPrefab.SetActive(false);
        inGameMenu.enabled = false;
        UICanvas.enabled = true;
        SetupUI();
        ui_btn_MultiMode.onClick.AddListener(OnClick_ui_btn_MultiMode);
        multiMode_btn_Login.onClick.AddListener(OnClick_multiMode_btn_Login);
    }

    public void OnClick_ui_btn_MultiMode()
    {
        UI_MainMenu.SetActive(false);
        UI_SingleMode.SetActive(false);
        UI_MultiMode.SetActive(true);
    }
    private void OnClick_multiMode_btn_Login()
    {
        var userName = multiMode_if_UserName.text == "" ? "default" : multiMode_if_UserName.text;
        var password = multiMode_if_Password.text == "" ? "12345" : multiMode_if_Password.text;
        
        var result = AccountBehavior.GetInstance().Login(userName, password);
        if (result == "" || !AccountBehavior.GetInstance().ConnectToServer())
        {
            //TODO: Handle case login false here
            return;
        }

        PlayerDataLocal.Init(result);
        UICanvas.enabled = false;
        
        inGameMenuPrefab.SetActive(true);
    }
}
