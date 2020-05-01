using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class SpawnerReceiver : MonoBehaviour
{
    [SerializeField] private GameObject [] SpawnableObjects;
    private ServerManager _serverManager;

    private void Start()
    {
        _serverManager = FindObjectOfType<ServerManager>();
    }

    public void Spawn(int objIndex, float givenHeight, float velx, float vely)
    {
        var newPos = Camera.main.ViewportToWorldPoint(new Vector3(1, givenHeight));
        newPos.z = 0;
        
        var newVelocity = new Vector2(velx, vely);
        newVelocity.Normalize();
        
        var newItem = Instantiate(SpawnableObjects[objIndex], newPos, Quaternion.identity);
        newItem.transform.right = -newVelocity;
        newItem.GetComponent<Rigidbody2D>().velocity = newVelocity;
    }
}
