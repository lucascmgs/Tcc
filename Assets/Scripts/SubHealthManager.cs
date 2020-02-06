using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubHealthManager : MonoBehaviour
{
    public int Health = 6;
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle")
        {
            var receivedDamage = other.gameObject.GetComponent<ObstacleDamageManager>().Damage;
            TakeDamage(receivedDamage);
        }
    }

    void TakeDamage(int receivedDamage = 1)
    {
        Health -= receivedDamage;
        if (Health < 0)
        {
            Health = 0;
        }
    }
}
