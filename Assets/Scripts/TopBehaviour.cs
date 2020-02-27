using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBehaviour : MonoBehaviour
{
    Rigidbody2D rb;
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        configure();
    }

    // Update is called once per frame
    void Update()
    {
        verifyToDestroy();
    }

    void configure()
    {
        rb.velocity = new Vector2(-1.5f, 0);
    }

    void verifyToDestroy()
    {
        if(this.transform.position.x <= -7)
        {
            Destroy(this.gameObject);
        }
    }
}
