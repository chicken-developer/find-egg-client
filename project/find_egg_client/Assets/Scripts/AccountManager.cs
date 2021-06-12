using System;
using System.IO;
using System.Net;
using UnityEngine;

public class AccountBehavior 
{
    private static AccountBehavior _instance;
    private string serverAddress = "http://192.168.1.9:8086/api/people";
    public static AccountBehavior GetInstance()
    {
        if (_instance == null)
        {
            _instance = new AccountBehavior();
        }
        return _instance;
    }
    public bool ConnectToServer()
    {
      //TODO: Check if not have internet 
        return true;
    }
    public string Login(string userName, string password)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format(serverAddress));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log("Result from account server: " + jsonResponse);
        return jsonResponse;
    }
    
}