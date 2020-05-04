using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class BroadcastSender : MonoBehaviour
    {
        [SerializeField] private float timeToBroadcast = 30f;
        
        private const int Port = 11000;

        private IEnumerator _coroutine;
        
        public TextMeshProUGUI status;

        [SerializeField] private StartManager startManager;

        private void Start()
        {
            if (startManager == null)
            {
                startManager = FindObjectOfType<StartManager>();
            }
        }

        public IEnumerator Broadcast()
        {

            Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            string hname = Dns.GetHostName();
            var ips = Dns.GetHostEntry(hname).AddressList;

            string myIp = "noIp";
            foreach (var ip in ips)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    myIp = ip.ToString();
                }
            }

            IPAddress broadcast = IPAddress.Parse("192.168.1.255");
            byte[] sendBuffer = Encoding.Unicode.GetBytes(myIp);

            IPEndPoint endpoint = new IPEndPoint(broadcast, Port);


            while (true)
            {
                mySocket.SendTo(sendBuffer, endpoint);

                status.text = "Starting Broadcast";

                if (status != null)
                {
                    status.text = "Broadcasting ip to " + endpoint.Address.ToString();
                }


                yield return new WaitForSeconds(0.5f);
            }

        }


        public void StartBroadCast()
        {
            startManager.DeployServer();
            _coroutine = Broadcast(); 
            StartCoroutine(_coroutine);
        }

        public void StopBroadCast()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }

        
            
        
    }
}