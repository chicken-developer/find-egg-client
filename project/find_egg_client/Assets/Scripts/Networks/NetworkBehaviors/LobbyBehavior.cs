namespace Networks.NetworkBehaviors
{
    public class LobbyBehavior: NetworkBehavior
    {
        private static LobbyBehavior _instance;
        
        public static LobbyBehavior GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LobbyBehavior();
            }
            return _instance;
        }

        public bool JoinLobby(Player player)
        {
            return true;
        }
    }
}