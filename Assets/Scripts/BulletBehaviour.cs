﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletBehaviour : MonoBehaviour
{
    public UnityEvent BulletRemoved;

    private void Start()
    {
        if (BulletRemoved == null)
        {
            BulletRemoved = new UnityEvent();    
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle" && other. name.Contains("Rock"))
        {
            Destroy(other.gameObject);
        }
        if(other.tag != "Player")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        RemoveBullet();
    }

    private void RemoveBullet()
    {
        BulletRemoved.Invoke();
        Destroy(this.gameObject);
    }
}
