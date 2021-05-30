using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class RealtimeDebug: MonoBehaviour
    {
        [SerializeField] private Text logText;
        
        private static RealtimeDebug _instance;
        
        public static RealtimeDebug GetInstance()
        {
            if (_instance == null)
            {
                _instance = new RealtimeDebug();
            }
            return _instance;
        }
        public void Append(string log)
        {
            string final_log = "\n" + log + "\n";
            logText.text += final_log;
        }
        
    }
}