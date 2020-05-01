using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{

    [SerializeField] private float speed = 6;
    [SerializeField] private bool destructible = false;
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
        
        
        
        var pos = Camera.main.WorldToViewportPoint(this.transform.position);

        if (pos.y > 0 && pos.y < 1 && pos.x < 1)
        {
            var height = Camera.main.WorldToViewportPoint(new Vector3(0, this.transform.position.y)).y;
            var velocity = this.GetComponent<Rigidbody2D>().velocity;
            
            
            
            client.Send("Spawn;" + itemId + ";" + height + ";" + velocity.x + ";" + velocity.y);
        }
        
        
        
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (destructible)
        {
            if (other.tag == "Bullet")
            {
                Destroy(this.gameObject);
            }

        }
    }
}
