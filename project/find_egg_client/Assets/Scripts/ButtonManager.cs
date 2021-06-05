
using UnityEngine;
using UnityEngine.UI;
public class ButtonManager: MonoBehaviour
{
    [SerializeField] private Canvas UICanvas;
    [SerializeField] private Canvas HUDCanvas;
    [SerializeField] private LobbyManager lobby;
    [SerializeField] private GameObject gameplay;
    
    [SerializeField] private GameObject UI_MainMenu;
    [SerializeField] private GameObject UI_SingleMode;
    [SerializeField] private GameObject UI_MultiMode;
    
    [SerializeField] private Button ui_btn_SingleMode;
    [SerializeField] private Button ui_btn_MultiMode;

    [SerializeField] private Button multiMode_btn_Login;
    [SerializeField] private InputField multiMode_if_UserName;
    [SerializeField] private InputField multiMode_if_Password;

    [SerializeField] private Button hud_btn_EnterGame;
    [SerializeField] private Button hud_btn_Shop;
    [SerializeField] private Text playerName;

    void Start()
    {
        lobby.enabled = true; // Enter lobby
        ui_btn_MultiMode.onClick.AddListener(OnClick_ui_btn_MultiMode);
        multiMode_btn_Login.onClick.AddListener(OnClick_multiMode_btn_Login);
        hud_btn_EnterGame.onClick.AddListener(OnClick_hud_btn_EnterGame);
        hud_btn_Shop.onClick.AddListener(OnClick_hud_btn_Shop);
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
        if (!AccountBehavior.GetInstance().Login(userName, password))
        {
            //TODO: Handle case login false here
            return;
        }

        playerName.text = userName;
        UICanvas.enabled = false;
        HUDCanvas.enabled = true;
    }

    private void OnClick_hud_btn_EnterGame()
    {
       
        UICanvas.enabled = false;
        HUDCanvas.enabled = false;
        lobby.enabled = true; // Enter lobby
        lobby.JoinOrQuitLobby(PlayerDataLocal.GenerationDataForLobby());
    }

    private void OnClick_hud_btn_Shop()
    {
       
       
        UICanvas.enabled = false;
        HUDCanvas.enabled = false;
       
    }
}
