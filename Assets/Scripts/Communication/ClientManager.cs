using System;
using System.ComponentModel;
using System.Net;
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
            try
            {
                Connect(ipInputField.text);
            }
            catch (Exception e)
            {
                GameObject status = GameObject.FindWithTag("Status");
                status.GetComponent<TextMeshProUGUI>().text = e.Message;
            }
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
            if (decodedMessage.Contains("phoneOwn"))
            {
                GameOptions.gameState = Gamestate.PhoneOwn;
            } else if (decodedMessage.Contains("subOwn"))
            {
                GameOptions.gameState = Gamestate.SubOwn;
            }

            SceneManager.LoadScene("GameOverPhoneScene");
        }

        if (decodedMessage.Contains("PlayAgain"))
        {
            GameOptions.gameState = Gamestate.NotStarted;
            SceneManager.LoadScene("SecondScreenScene");
        }

        if (decodedMessage.Contains("Quit"))
        {
            Destroy(this.gameObject);
            SceneManager.LoadScene("StartScene");
        }

    }

    private void OnDestroy()
    {
        _client.Disconnect();
    }
}