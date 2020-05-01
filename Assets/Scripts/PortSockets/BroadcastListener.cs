using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;


public class BroadcastListener : MonoBehaviour
{
    private const int Port = 11000;

    private UdpClient listener;

    private IPEndPoint groupEp;

    private bool _started = false;

    public Text status;

    [SerializeField] private StartManager startManager;

    public void StartListener()
    {
        if (startManager == null)
        {
            startManager = FindObjectOfType<StartManager>();
        }

        if (!_started)
        {
            startManager.DeployClient();
            listener = new UdpClient(Port);
            groupEp = new IPEndPoint(IPAddress.Any, Port);
            _started = true;
            
            Listen();

        }
    }

    public void CloseListener()
    {
        if (listener != null)
        {
            listener.Close();
        }
        _started = false;
    }

    private async void Listen()
    {
        if (_started)
        {
            try
            {
                UdpReceiveResult bytes = await listener.ReceiveAsync();

                string decoded = Encoding.Unicode.GetString(bytes.Buffer, 0, bytes.Buffer.Length);
                if (decoded != "")
                {
                    status.text = "ReceivedIp: " + decoded;
                    Debug.Log(decoded);
                    CloseListener();
                    startManager.DeployClient();
                    startManager.InstantiatedClient.GetComponent<ClientManager>().Connect(decoded);
                }
            }
            catch (SocketException e)
            {
                Debug.LogError("Erro de socket: " + e);
                CloseListener();
            }
        }
    }
    
    
}