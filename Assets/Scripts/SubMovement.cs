using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SubMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float horizontalSpeed = 5f;
    [SerializeField]
    private float verticalSpeed = 3f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
      
    }

    public void Update()
    {
    }


    public void Move(InputAction.CallbackContext context)
    {
        var movement = context.ReadValue<Vector2>();
        movement.x *= horizontalSpeed;
        movement.y *= verticalSpeed;

        rb.velocity = movement;   

    }
    
}
