using System;
using System.ComponentModel;
using System.Text;
using DefaultNamespace;
using UnityEngine;
using Telepathy;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EventType = Telepathy.EventType;

public class ClientManager : ManagerBase
{
    private bool _sceneChanged = false;

    public Client _client = new Client();

    private bool hasConnected = false;

    private void Awake()
    {
        
        TMP_InputField ipInputField = FindObjectOfType<TMP_InputField>();
        if (ipInputField != null && ipInputField.text != "")
        {
            Connect(ipInputField.text);
        }
        _device = _client;
        Setup();
    }


    void Update()
    {
        if (_client.Connected)
        {
            hasConnected = true;
            CheckMessages();
            Send("Ok");
        }
        else
        {
            if (hasConnected)
            {
                ManageConnectionStopped();
            }
        }
    }

    public void Connect(string givenAddress)
    {
        if (!givenAddress.Contains("192.168.1"))
        {
            givenAddress = "192.168.1." + givenAddress;
        }
        
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

    protected override void DecodeMessage(byte[] data)
    {
        string decodedMessage = Encoding.Unicode.GetString(data);
        if (decodedMessage.Contains("EndGame"))
        {
            
        }

    }

    private void OnDestroy()
    {
        _client.Disconnect();
    }
}