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
    [SerializeField]
    GameObject floorPrefab;
    [SerializeField]
    GameObject ceilingPrefab;

    int resolutionWidth;
    int resolutionHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        resolutionWidth = Screen.currentResolution.width;
        resolutionHeight = Screen.currentResolution.height;
    }

    public void Update()
    {
        checkMovement();
    }

    public void Move(InputAction.CallbackContext context)
    {
        var movement = context.ReadValue<Vector2>();
        var limits = Camera.main.WorldToViewportPoint(this.transform.position);

        movement.x *= horizontalSpeed;
        movement.y *= verticalSpeed;

        rb.velocity = movement;
    }

    void checkMovement()
    {
        
        if (this.transform.position.y + ceilingPrefab.GetComponent<SpriteRenderer>().size.y / 2 >= Camera.main.orthographicSize)
        {
            this.transform.position = new Vector2(this.transform.position.x, Camera.main.orthographicSize - ceilingPrefab.GetComponent<SpriteRenderer>().size.y / 2);
        }

        else if(this.transform.position.y - floorPrefab.GetComponent<SpriteRenderer>().size.y <= -Camera.main.orthographicSize)
        {
            this.transform.position = new Vector2(this.transform.position.x, -Camera.main.orthographicSize + floorPrefab.GetComponent<SpriteRenderer>().size.y);
        }

        if (this.transform.position.x + this.GetComponent<SpriteRenderer>().size.x >= ( (Camera.main.orthographicSize * resolutionWidth) / resolutionHeight ))
        {
            this.transform.position = new Vector2(((Camera.main.orthographicSize * resolutionWidth) / resolutionHeight ) - this.GetComponent<SpriteRenderer>().size.x, this.transform.position.y);
        }

        else if (this.transform.position.x - this.GetComponent<SpriteRenderer>().size.x <= ( -(Camera.main.orthographicSize * resolutionWidth) / resolutionHeight))
        {
            this.transform.position = new Vector2(-((Camera.main.orthographicSize * resolutionWidth) / resolutionHeight) + this.GetComponent<SpriteRenderer>().size.x, this.transform.position.y);
        }

    }

}
