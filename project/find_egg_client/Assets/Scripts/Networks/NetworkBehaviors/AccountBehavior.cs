
namespace Networks.NetworkBehaviors
{
    public class AccountBehavior: NetworkBehavior
    {
        //Using rest api only
        private static AccountBehavior _instance;
        
        public static AccountBehavior GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AccountBehavior();
            }
            return _instance;
        }
        public bool Login(string userName, string password)
        {
            return true;
        }

        public bool Register()
        {
            return true;
        }
        
    }
}