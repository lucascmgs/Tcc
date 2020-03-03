using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBehaviour : MonoBehaviour
{
    Rigidbody2D rb;
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

}
