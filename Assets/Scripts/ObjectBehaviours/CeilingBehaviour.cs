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
    [SerializeField] GameObject backgroundPrefab;

    GameObject backgroundComponent;

    [SerializeField] private Queue<GameObject> ceilingList;

    GameObject newCeiling;
    GameObject newFloor;
    GameObject newBg;

    public float timeToIncreaseVelocity;
    float velocityTimeElapsed;

    float horizontalSize;
    int count;

    void Start()
    {
        backgroundComponent = FindObjectOfType<BackgroundBehaviour>().gameObject;

        ceilingList = new Queue<GameObject>();

        newCeiling = Instantiate(ceilingPrefab, new Vector2(this.transform.position.x, Camera.main.orthographicSize), Quaternion.identity);
        newCeiling.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);

        newFloor = Instantiate(ceilingPrefab, new Vector2(this.transform.position.x, -Camera.main.orthographicSize), Quaternion.identity);
        newFloor.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);

        backgroundComponent.transform.position = new Vector2(this.transform.position.x, -Camera.main.orthographicSize + 0.3f);
        newBg = Instantiate(backgroundPrefab, backgroundComponent.transform.position, Quaternion.identity);
        newBg.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);

        newCeiling.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        newFloor.gameObject.GetComponent<SpriteRenderer>().sprite = null;
        newBg.gameObject.GetComponent<SpriteRenderer>().sprite = null;

        velocityTimeElapsed = Time.time;
    }

    void Update()
    {
        generateMap();
    }

    void generateMap()
    {

        if (newCeiling.transform.position.x + 1 <= this.transform.position.x)
        {
            var rand = new System.Random();

            newCeiling = Instantiate(ceilingPrefab, new Vector2(newCeiling.transform.position.x + 1, Camera.main.orthographicSize), Quaternion.identity, this.transform);
            newCeiling.gameObject.GetComponent<SpriteRenderer>().sprite = ceilingSprites[rand.Next(0,4)];
            newCeiling.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);
            ceilingList.Enqueue(newCeiling);

            newFloor = Instantiate(floorPrefab, new Vector2(newFloor.transform.position.x + 1, -Camera.main.orthographicSize), Quaternion.identity, this.transform);
            newFloor.gameObject.GetComponent<SpriteRenderer>().sprite = floorSprites[rand.Next(0,4)];
            newFloor.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);
            ceilingList.Enqueue(newFloor);

            newBg = Instantiate(backgroundPrefab, new Vector3(newBg.transform.position.x + 1, backgroundComponent.transform.position.y, 1), Quaternion.identity, backgroundComponent.transform);
            newBg.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);

            tryToCreateStalactite();
            tryToCreateCeiling();

            if(ceilingList.Count >= 40) { 
                Destroy(ceilingList.Dequeue());
                Destroy(ceilingList.Dequeue());
            }

            //updateVelocity();

        }
    }

    void tryToCreateStalactite()
    {
        if (UnityEngine.Random.value < randValue) 
        {
            var stalactite = Instantiate(stalactitePrefab, new Vector3(newCeiling.transform.position.x, Camera.main.orthographicSize, 4), Quaternion.identity, newCeiling.transform);
            stalactite.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel , 0 );
        }
    }

    void tryToCreateCeiling()
    {
        if (UnityEngine.Random.value <= 0.5f) {
            var ceiling = Instantiate(ceilingPrefab, new Vector3(newCeiling.transform.position.x, Camera.main.orthographicSize, 8), Quaternion.identity, newCeiling.transform);
            ceiling.gameObject.GetComponent<SpriteRenderer>().sprite = stalacniteSprites[0];
            ceiling.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);
        }

        else if (UnityEngine.Random.value > 0.5f)
        {
            var ceiling = Instantiate(ceilingPrefab, new Vector3(newCeiling.transform.position.x, Camera.main.orthographicSize, 6), Quaternion.identity, newCeiling.transform);
            ceiling.gameObject.GetComponent<SpriteRenderer>().sprite = stalacniteSprites[1];
            ceiling.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);
        }
    }

    void updateVelocity()
    {
        if(velocityTimeElapsed + timeToIncreaseVelocity < Time.time)
        {
            vel += 0.2f;

            foreach(GameObject go in ceilingList)
            {
                go.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);
            }

            velocityTimeElapsed = Time.time;
        }
    }

}
