using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviourPunCallbacks, IPunObservable
{

    [SerializeField] private float speed = 6;
    [SerializeField] private bool destructible = false;
    private float Height;
    private Vector2 Velocity;
    private bool transferred = false;
    private SpriteRenderer render;


    private void Start()
    {
        render = this.GetComponent<SpriteRenderer>();
        if (PhotonNetwork.IsMasterClient)
        {
            render.enabled = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    public void OnBecameInvisible()
    {

        if (!PhotonNetwork.IsMasterClient)
        {
            var pos = Camera.main.WorldToViewportPoint(this.transform.position);

            if (pos.y > 0 && pos.y < 1 && pos.x < 1)
            {
                var height = Camera.main.WorldToViewportPoint(new Vector3(0, this.transform.position.y)).y;
                var velocity = this.GetComponent<Rigidbody2D>().velocity;
                this.photonView.RPC("Transfer", RpcTarget.MasterClient, height, velocity);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (destructible)
        {
            if (other.tag == "Bullet")
            {
                Destroy(this.gameObject);
            }

        }
    }

    [PunRPC]
    public void Transfer(float givenHeight, Vector2 givenVelocity)
    {
        render.enabled = true;
        var newPos = Camera.main.ViewportToWorldPoint(new Vector3(1, givenHeight));

        givenVelocity.Normalize();

        newPos.z = 0;
        this.transform.position = newPos;
        var rb = this.GetComponent<Rigidbody2D>();
        this.transform.right = -givenVelocity;
        rb.velocity = givenVelocity * speed;

    }
}
