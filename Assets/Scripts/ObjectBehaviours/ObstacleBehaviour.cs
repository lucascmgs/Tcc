using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.ObjectBehaviours;
using UnityEngine;

public class ObstacleBehaviour : DamageableBehaviour
{

    public float speed = 6;
    [SerializeField] private int itemId;
    private float Height;
    private Vector2 Velocity;
    private bool transferred = false;
    private SpriteRenderer render;


    private void Start()
    {
        render = this.GetComponent<SpriteRenderer>();
        
    }


    public void OnBecameInvisible()
    {
        var server = FindObjectOfType<ServerManager>();

        if (server != null)
        {
            Destroy(this.gameObject);
            return;
        }
        
        
        var client = FindObjectOfType<ClientManager>();
        if (client == null)
        {
            return;
        }
        
        
        var pos = Camera.main.WorldToViewportPoint(this.transform.position);

        if (pos.y > 0 && pos.y < 1 && pos.x < 1)
        {
            var height = Camera.main.WorldToViewportPoint(new Vector3(0, this.transform.position.y)).y;
            var velocity = this.GetComponent<Rigidbody2D>().velocity;


            string envio = "Spawn;" + itemId + ";" + height + ";" + velocity.x + ";" + velocity.y;
            Debug.Log(envio);
            client.Send(envio);
        }
        
        
        
        Destroy(this.gameObject);
    }


    
}