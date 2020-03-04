using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CeilingBehaviour : MonoBehaviour
{

    public float vel;
    public float randValue;

    public Sprite[] ceilingSprites;
    public Sprite[] floorSprites;
    public Sprite[] stalacniteSprites;

    [SerializeField] GameObject ceilingPrefab;
    [SerializeField] GameObject floorPrefab;
    [SerializeField] GameObject stalactitePrefab;

    GameObject newCeiling;
    GameObject newFloor;

    float timeToInstatianteStalactite;
    float stalactiteTimeElapsed;

    float timeToInstantiateCeiling;
    float ceilingTimeElapsed;

    void Start()
    {
        newCeiling = Instantiate(ceilingPrefab, new Vector2(this.transform.position.x, Camera.main.orthographicSize), Quaternion.identity);
        newCeiling.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);

        newFloor = Instantiate(ceilingPrefab, new Vector2(this.transform.position.x, -Camera.main.orthographicSize), Quaternion.identity);
        newFloor.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);
        
        newCeiling.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        newFloor.gameObject.GetComponent<SpriteRenderer>().sprite = null;

        stalactiteTimeElapsed = Time.time;
        timeToInstatianteStalactite = 1 / vel;

        ceilingTimeElapsed = Time.time;
        timeToInstantiateCeiling = 1 / vel;
    }

    void Update()
    {
        createCelling();
        tryToCreateStalactite();
        tryToCreateCeiling();
    }

    void createCelling()
    {

        if (newCeiling.transform.position.x + 1 <= this.transform.position.x)
        {
            var rand = new System.Random();

            newCeiling = Instantiate(ceilingPrefab, new Vector2(newCeiling.transform.position.x + 1, Camera.main.orthographicSize), Quaternion.identity);
            newCeiling.gameObject.GetComponent<SpriteRenderer>().sprite = ceilingSprites[rand.Next(0,4)];
            newCeiling.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);

            newFloor = Instantiate(floorPrefab, new Vector2(newFloor.transform.position.x + 1, -Camera.main.orthographicSize), Quaternion.identity);
            newFloor.gameObject.GetComponent<SpriteRenderer>().sprite = floorSprites[rand.Next(0, 4)];
            newFloor.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);

        }
    }

    void tryToCreateStalactite()
    {

        if (stalactiteTimeElapsed + timeToInstatianteStalactite < Time.time)
        {
            if (UnityEngine.Random.value < randValue) { stalactiteTimeElapsed = Time.time; }

            else
            {
                var stalactite = Instantiate(stalactitePrefab, new Vector3(newCeiling.transform.position.x, Camera.main.orthographicSize, 4), Quaternion.identity);
                stalactite.GetComponent<Rigidbody2D>().velocity = new Vector2( -vel , 0 );
                stalactiteTimeElapsed = Time.time;
            }
        }

    }

    void tryToCreateCeiling()
    {
        if (ceilingTimeElapsed + timeToInstantiateCeiling < Time.time)
        {
            if (UnityEngine.Random.value <= 0.25f) {
                var ceiling = Instantiate(ceilingPrefab, new Vector3(newCeiling.transform.position.x, Camera.main.orthographicSize, 8), Quaternion.identity);
                ceiling.gameObject.GetComponent<SpriteRenderer>().sprite = stalacniteSprites[0];
                ceiling.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);
            }

            else if (UnityEngine.Random.value > 0.25f && UnityEngine.Random.value < 0.5f)
            {
                var ceiling = Instantiate(ceilingPrefab, new Vector3(newCeiling.transform.position.x, Camera.main.orthographicSize, 8), Quaternion.identity);
                ceiling.gameObject.GetComponent<SpriteRenderer>().sprite = stalacniteSprites[0];
                ceiling.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);

                ceiling = Instantiate(ceilingPrefab, new Vector3(newCeiling.transform.position.x, Camera.main.orthographicSize, 6), Quaternion.identity);
                ceiling.gameObject.GetComponent<SpriteRenderer>().sprite = stalacniteSprites[1];
                ceiling.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);

            }

            else if (UnityEngine.Random.value >= 0.5f && UnityEngine.Random.value < 0.75f)
            {
                var ceiling = Instantiate(ceilingPrefab, new Vector3(newCeiling.transform.position.x, Camera.main.orthographicSize, 6), Quaternion.identity);
                ceiling.gameObject.GetComponent<SpriteRenderer>().sprite = stalacniteSprites[1];
                ceiling.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);
            }

            ceilingTimeElapsed = Time.time;
        }
    }

}
