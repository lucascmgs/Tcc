using System;
using System.ComponentModel;
using System.Text;
using DefaultNamespace;
using UnityEngine;
using Telepathy;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EventType = Telepathy.EventType;

public class ClientManager : ManagerBase
{

 private bool _sceneChanged = false;
    
    public Client _client = new Client();
   
    
    
    private void Awake()
    {
        _device = _client;
        Setup();
    }

    private void Start()
    {
    }

    void Update()
    {
        if (_client.Connected)
        {
            
               CheckMessages();
               Send("Ok");
        }
    }
    public void Connect(string givenAddress)
    {
        Debug.Log("Connecting");
        _client.Connect(givenAddress, Port);
    }

    public void Send(string message)
    {
        _client.Send(Encoding.Unicode.GetBytes(message));
    }

    protected override void ManageConnectionStart(int connectionId)
    {
        SceneManager.LoadScene("SecondScreenScene");
    }
    

    private void OnDestroy()
    { 
        _client.Disconnect();
    }
    
    
}
