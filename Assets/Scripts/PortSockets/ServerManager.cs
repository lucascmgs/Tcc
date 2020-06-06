using UnityEngine;
using System;
using System.Text;
using System.Threading;
using Telepathy;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class ServerManager : ManagerBase
    {
        public Server _server = new Server();
        private bool _sceneChanged = false;

        private int clientConnectionId = -1;

        private SpawnerReceiver spawnerReceiver;

        private void Awake()
        {
            _device = _server;
            Setup();
        }

        private void Start()
        {
            Host();
        }

        private void Update()
        {
            if (_server.Active)
            {
                CheckMessages();
                if (clientConnectionId != -1)
                {
                    Send("Ok");
                }
            }
        }

        public void Host()
        {
            Debug.Log("Starting Server");
            _server.Start(Port);
        }

        public void Send(string message)
        {
            if (clientConnectionId != -1)
            {
                _server.Send(clientConnectionId, Encoding.Unicode.GetBytes(message));
            }
        }

        protected override void ManageConnectionStart(int connectionId)
        {
            clientConnectionId = connectionId;
            Debug.Log("Started Server");
            SceneManager.LoadScene("MainGameScene");
        }

        protected override void DecodeMessage(byte[] data)
        {
            string decodedMessage = Encoding.Unicode.GetString(data);

            if (decodedMessage.Contains("Spawn"))
            {
                if (spawnerReceiver == null)
                {
                    spawnerReceiver = FindObjectOfType<SpawnerReceiver>();
                }

                decodedMessage.Replace(".", ",");
                //Debug.Log(decodedMessage);
                var splitResult = decodedMessage.Split(';');
                var res4 = splitResult[4];
                var newRes = float.Parse(res4);
                spawnerReceiver.Spawn(int.Parse(splitResult[1]), float.Parse(splitResult[2]), float.Parse(splitResult[3]), float.Parse(splitResult[4]));
            }
        }


        private void OnDestroy()
        {
            _server.Stop();
        }
    }
}