using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class StalactiteBehaviour : MonoBehaviour
{
    public float randomValueToFall;
    private float timeElapsed = 0;
    public float speed;

    private Animator _anim;
    
    private Rigidbody2D _rb;

    private Collider2D _col;

    private AudioManager _audioManager;
    
    bool verify;
    bool alreadyFallen;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        verify = true;
        alreadyFallen = false;
        _anim = GetComponent<Animator>();
        
        _col = GetComponent<Collider2D>();
        
        _audioManager = FindObjectOfType<AudioManager>();

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

        checkXPosition();

    }

    void checkIfFalling()
    {
        if(UnityEngine.Random.value > randomValueToFall)
        {
            this.transform.parent = null;
            _rb.velocity = new Vector2(_rb.velocity.x, -speed);
            alreadyFallen = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            Break();
        }
        else if (collision.tag == "Floor")
        {
            this._rb.velocity = new Vector2(_rb.velocity.x, 0);
        }
    }

    private void Break()
    {
        Debug.Log("OK");
        _col.enabled = false;
        this._rb.velocity = new Vector2(this._rb.velocity.x, this._rb.velocity.y / 10);
        _audioManager.Play("BrokenStalactite");
        
        _anim.SetBool("Broken", true);
    }

    private void OnBecameInvisible()
    {
        var pos = new Vector3();
        try
        {
            pos = Camera.main.WorldToViewportPoint(this.transform.position);

        }
        catch (Exception e){}
        

        if(pos.x <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void checkXPosition()
    {
        if (this.transform.position.x <= -6) Destroy(this.gameObject);
    }
}
