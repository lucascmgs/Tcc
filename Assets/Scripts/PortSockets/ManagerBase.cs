using UnityEngine;
using System;
using Telepathy;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class ManagerBase : MonoBehaviour
    {
        protected Telepathy.Common _device;
        protected const int Port = 1337;
        public const float timeOut = 1f;
        private float lastMessageTime = 0f;

        protected void Setup()
        {
            DontDestroyOnLoad(this);
            Application.runInBackground = true;
            Telepathy.Logger.Log = Debug.Log;
            Telepathy.Logger.LogWarning = Debug.LogWarning;
            Telepathy.Logger.LogError = Debug.LogError;
        }

        protected void CheckMessages()
        {
            Telepathy.Message msg;

            float timeSinceLastMessage = Time.time - lastMessageTime;

            if (timeSinceLastMessage > timeOut && lastMessageTime > 0)
            {
                ManageConnectionStopped();
            }
            else
            {
                if (lastMessageTime > 0)
                {
                    lastMessageTime = Time.time;
                }

                while (_device.GetNextMessage(out msg))
                {
                    switch (msg.eventType)
                    {
                        case Telepathy.EventType.Connected:
                            lastMessageTime = Time.time;
                            ManageConnectionStart(msg.connectionId);
                            Debug.Log("Connected");
                            break;
                        case Telepathy.EventType.Data:
                            DecodeMessage(msg.data);
                            break;
                        case Telepathy.EventType.Disconnected:
                            ManageConnectionStopped();
                            Debug.Log("Disconnected");
                            break;
                    }
                }
            }
        }

        protected virtual void ManageConnectionStart(int connectionId)
        {
        }

        protected void ManageConnectionStopped()
        {
            Destroy(this);
            SceneManager.LoadScene(0);
        }

        protected virtual void DecodeMessage(byte[] data)
        {
        }
    }
}