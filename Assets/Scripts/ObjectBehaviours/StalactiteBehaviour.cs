using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactiteBehaviour : MonoBehaviour
{
    public float randomValueToFall;
    private Rigidbody2D rb;
    private float timeElapsed = 0;
    public float speed;
    bool verify;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-1.5f, 0);
        verify = true;
    }
    void Update()
    {
        if (verify)
        {
            checkIfFalling();
            timeElapsed = Time.time;
            verify = false;
        }
        else { 
            if(Time.time - timeElapsed > 0.5) { 
                verify = true;
            }
        }

    }

    void checkIfFalling()
    {
        if(UnityEngine.Random.value > randomValueToFall)
        {
            rb.velocity = new Vector2(-1.5f, -speed);
        }
    }
}
