using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;


public class GunBehaviour : MonoBehaviour
{
    [SerializeField] private float mouseTreshold = 0.25f;

    [SerializeField] private float bulletSpeed = 2;

    public int maxBullets = 3;

    [SerializeField] private GameObject bulletPrefab;


    private Pointer currentMouse;

    private Camera mainCamera;

    private Vector2 directionVector;

    [NonSerialized] public int bulletCount = 0;

    private AudioManager _audioManager;


    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        currentMouse = Mouse.current;
        mainCamera = Camera.main;
        StartCoroutine(TestCoroutine());
    }

    void Update()
    {
        PointToMouse();
    }

    void PointToMouse()
    {
        var mousePos = mainCamera.ScreenToWorldPoint(currentMouse.position.ReadValue());
        directionVector = new Vector2(mousePos.x - this.transform.position.x, mousePos.y - this.transform.position.y);

        if (directionVector.SqrMagnitude() > mouseTreshold)
        {
            this.transform.right = directionVector;
        }

        directionVector.Normalize();
    }

    public void Fire(InputAction.CallbackContext context)
    {
        Debug.Log(bulletCount);
        if (context.performed)
        {
            if (bulletCount < maxBullets)
            {
                _audioManager.Play("Shoot");
                var newPos = new Vector3(this.transform.position.x + directionVector.x / 2,
                    this.transform.position.y + directionVector.y / 2, -1);
                var newBullet = Instantiate(bulletPrefab, newPos, Quaternion.identity);
                newBullet.GetComponent<BulletBehaviour>().BulletRemoved.AddListener(DecreaseBulletCount);

                newBullet.GetComponent<Rigidbody2D>().velocity = directionVector * bulletSpeed;
                ;
                newBullet.transform.right = directionVector;
                bulletCount++;
            }
        }
    }

    public void DecreaseBulletCount()
    {
        bulletCount--;
    }


    private IEnumerator TestCoroutine()
    {
        int vezes = 0;
        while (true)
        {
            Debug.Log("Foi " + vezes);
            vezes++;
            yield return new WaitForSeconds(1.0f);
        }
    }
}