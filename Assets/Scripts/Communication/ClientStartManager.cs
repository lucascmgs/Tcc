using System;
using System.Collections;
using System.Collections.Generic;
using Telepathy;
using UnityEngine;

public class ClientStartManager : StartManagerBase
{
    public GameObject clientPrefab;
    [NonSerialized]
    public GameObject InstantiatedClient;
    
    public void Awake()
    {
        var clientManager = FindObjectOfType<ClientManager>();
        if (clientManager != null)
        {
            Destroy(clientManager.transform.parent);
        }
        
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR", false);
        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR", false);
    }

    public override void DeployStartManager()
    {
        if (clientPrefab != null && InstantiatedClient == null)
        {
            InstantiatedClient = Instantiate(clientPrefab);
        }
    }

    protected override void DestroyStartManager()
    {
        if (InstantiatedClient != null)
        {
            Destroy(InstantiatedClient);
        }
    }
}
