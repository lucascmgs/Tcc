using System;
using System.ComponentModel;
using System.Text;
using DefaultNamespace;
using UnityEngine;
using Telepathy;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EventType = Telepathy.EventType;

public class ClientManager : ManagerBase
{
    private bool _sceneChanged = false;

    public Client _client = new Client();

    private bool hasConnected = false;

    public LevelEvent levelEvent;

    public class LevelEvent : UnityEvent<int>
    {
    }


    private void Awake()
    {
        levelEvent = new LevelEvent();

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
            if (decodedMessage.Contains("phoneOwn"))
            {
                GameOptions.gameState = Gamestate.PhoneOwn;
            }
            else if (decodedMessage.Contains("subOwn"))
            {
                GameOptions.gameState = Gamestate.SubOwn;
            }

            SceneManager.LoadScene("GameOverPhoneScene");
        }

        if (decodedMessage.Contains("ComboLevel"))
        {
            Debug.Log("Chegou combo level");
            string[] splitMsg = decodedMessage.Split(';');
            int level = Int32.Parse(splitMsg[1]);
            levelEvent.Invoke(level);
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