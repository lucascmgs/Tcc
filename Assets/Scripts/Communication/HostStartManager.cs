using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using DefaultNamespace;
using Telepathy;
using TMPro;
using UnityEngine;

public class HostStartManager : StartManagerBase
{
    public GameObject serverPrefab;
    public TextMeshProUGUI localIp;
    
    [NonSerialized]
    public GameObject InstantiatedServer;
    
    public void Awake()
    {
        var serverManager = FindObjectOfType<ServerManager>();
        if (serverManager != null)
        {
            Destroy(serverManager.transform.parent);
        }
        
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR", false);
        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR", false);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (localIp == null)
        {
            return;
        }
        
        string hname = Dns.GetHostName();
        var ips = Dns.GetHostEntry(hname).AddressList;

        var interfaces =  NetworkInterface.GetAllNetworkInterfaces();
        localIp.text = "Local IP:";
        foreach (var ip in ips)
        {

            if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString().Contains("192.168"))
            {
                localIp.text += "\n" + ip.ToString();
            }
        }
    }

    public override void DeployStartManager()
    {
        if (serverPrefab != null && InstantiatedServer == null) 
        {
            InstantiatedServer = Instantiate(serverPrefab);
        }
    }

    protected override void DestroyStartManager()
    {
        if (InstantiatedServer != null)
        {
            Destroy(InstantiatedServer);
        }
    }
   
    public void SetMusic()
    {
        GameOptions.playMusic = !GameOptions.playMusic;
    }

    public void SetSound()
    {
        GameOptions.playSounds = !GameOptions.playSounds;
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }
}
