using System;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            Destroy(other.gameObject);
            RemoveBullet();
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
