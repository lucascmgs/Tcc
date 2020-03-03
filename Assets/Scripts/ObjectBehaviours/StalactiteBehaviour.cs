using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactiteBehaviour : MonoBehaviour
{
    public float randomValueToFall;
    private float timeElapsed = 0;
    public float speed;

    private Rigidbody2D rb;
    
    bool verify;
    bool alreadyFallen;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        verify = true;
        alreadyFallen = false;
    }
    void Update()
    {
        if (verify && !alreadyFallen)
        {
            checkIfFalling();
            timeElapsed = Time.time;
            verify = false;
        }
        else { 
            if(Time.time - timeElapsed > 1) { 
                verify = true;
            }
        }

    }

    void checkIfFalling()
    {
        if(UnityEngine.Random.value > randomValueToFall)
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);
            alreadyFallen = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet") Destroy(this.gameObject);
        else if (collision.tag == "Floor") this.rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    private void OnBecameInvisible()
    {
        var pos = Camera.main.WorldToViewportPoint(this.transform.position);

        if(pos.x <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
