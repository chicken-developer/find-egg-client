namespace Networks
{
    public class Player
    {
        private static Player _instance;
        public bool isVerify = false;
        
        private static string userName;
        private static string password;
        private static string emailAddress;
        
        public static Player GetInstance()
        {
            if (_instance == null)
                _instance = new Player();
            return _instance;
        }

        public bool checkVerify()
        {
            isVerify = true; //TODO: Create check verify data here;
            return isVerify;
        }
        public void setUserName(string username)
        {
            userName = username;
        }

        public string getUserName()
        {
            return userName;
        }

        public void setPassword(string passwd)
        {
            password = passwd;
        }

        public void setEmail(string email)
        {
            emailAddress = email;
        }

        public string getEmail()
        {
            return emailAddress;
        }
    }
}