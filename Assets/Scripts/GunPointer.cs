using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GunPointer : MonoBehaviour
{
    [SerializeField]
    private float mouseTreshold = 0.25f;

    private Pointer currentMouse;

    private void Start()
    {
        currentMouse = Mouse.current;
    }

    void Update()
    {   
        PointToMouse();
    }

    void PointToMouse()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(currentMouse.position.ReadValue());
        Vector2 directionVector = new Vector2(mousePos.x - this.transform.position.x, mousePos.y - this.transform.position.y);

        if(directionVector.SqrMagnitude() > mouseTreshold){
            this.transform.right = directionVector;
        }
    }
}
