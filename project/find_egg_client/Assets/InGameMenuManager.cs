using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class InGameMenuManager : MonoBehaviour
{
    
    
    [SerializeField] private Button btnSwitchSkin;
    [SerializeField] private InputField inputFRoomID;
    [SerializeField] private Button btnEnterPrivateGame;
    [SerializeField] private Button btnEnterRandomGame;
    [SerializeField] private Button btnShop;
    [SerializeField] private Button btnLogout;
    
    [SerializeField] private GameObject inGameMenuPrefab;
    [SerializeField] private GameObject gameplayPrefab;
    [SerializeField] private GameObject lobbyPrefab;
    void Start()
    {
        btnEnterRandomGame.onClick.AddListener(OnClick_EnterRandomGame);
    }
    // Update is called once per frame
    void Update()
    {

    }

    void OnClick_EnterRandomGame()
    {
        lobbyPrefab.SetActive(true); // Enter lobby
        lobbyPrefab.GetComponent<LobbyManager>().JoinOrQuitLobby(PlayerDataLocal.GenerationDataForLobby());
    }
}
