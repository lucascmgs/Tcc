using System;
using System.Collections;
using System.Collections.Generic;
using Telepathy;
using UnityEngine;
using Random = System.Random;

public class SpawnerSender : MonoBehaviour
{
   [SerializeField] private GameObject SpawnableObject;

   private ClientManager clientManager;

   private float x = 0;
   private float y = 0;

   private Random random;
   
   private void Start()
   {
      random = new Random();
      clientManager = FindObjectOfType<ClientManager>();
   }

   public void SendSpawn()
   {
      clientManager.Send($"Spawn;{x};{y}");
      x = random.Next(-7, 7);
      y = random.Next(-5, 5);
   }
}
