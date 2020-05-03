using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using DefaultNamespace;
using Telepathy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public GameObject clientPrefab;
    public GameObject serverPrefab;
    public TextMeshProUGUI localIp;
    
    [NonSerialized]
    public GameObject InstantiatedServer;
    [NonSerialized]
    public GameObject InstantiatedClient;

    public Button hostButton;
    public Button connectButton;
    public Button stopButton;

    public void Awake()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR", false);
        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR", false);
    }

    public void Start()
    {
        string hname = Dns.GetHostName();
        var ips = Dns.GetHostEntry(hname).AddressList;
        
        localIp.text = "Local IP:";
        foreach (var ip in ips)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIp.text += "\n" + ip.ToString();
            }
        }
       
    }
    
    public void DeployClient()
    {
        if (clientPrefab != null && InstantiatedServer == null && InstantiatedClient == null)
        {
            InstantiatedClient = Instantiate(clientPrefab);
        }
    }

    public void DestroyClient()
    {
        if (InstantiatedClient != null)
        {
            Destroy(InstantiatedClient);
        }
    }

    public void DeployServer()
    {
        if (serverPrefab != null && InstantiatedClient == null && InstantiatedServer == null) 
        {
            InstantiatedServer = Instantiate(serverPrefab);
        }
    }

    public void DestroyServer()
    {
        if (InstantiatedServer != null)
        {
            Destroy(InstantiatedServer);
        }
    }
    
    public void DeactivateHostAndConnectButtons()
    {
        hostButton.interactable = false;
        connectButton.interactable = false;
        stopButton.gameObject.SetActive(true);
    }

    public void ResetButtons()
    {
        hostButton.interactable = true;
        connectButton.interactable = true;
        stopButton.gameObject.SetActive(false);
    }
}