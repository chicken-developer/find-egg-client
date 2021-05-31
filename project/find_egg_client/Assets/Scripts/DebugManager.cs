
using Networks;
using Networks.NetworkBehaviors;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
     
    [SerializeField] private Button btn_test_ScalaServer;
    [SerializeField] private Button btn_test_CppServer;
    [SerializeField] private Text debugTxt;
    [SerializeField] private Button btn_test_GolangServer;
    // Start is called before the first frame update
    void Start()
    {
        btn_test_ScalaServer.onClick.AddListener(OnClick_btn_test_ScalaServer);
        btn_test_CppServer.onClick.AddListener(OnClick_btn_test_CPPServer);
        btn_test_GolangServer.onClick.AddListener(OnClick_btn_test_GolangServer);
    }
    private void OnClick_btn_test_ScalaServer()
    {
        CoreGameBehavior.GetInstance().EnterGame(Player.GetInstance());
    }
    private void OnClick_btn_test_CPPServer()
    {
        LobbyBehavior.GetInstance().JoinLobby(Player.GetInstance());
    }
    private void OnClick_btn_test_GolangServer()
    {
        AccountBehavior.GetInstance().Login("usernamedemo","passdemo");
    }
  
}
