using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class CeilingBehaviour : MonoBehaviour
{

    public float vel;
    public float randValue;

    public Sprite[] ceilingSprites;
    public Sprite[] floorSprites;
    public Sprite[] stalactiteSprites;
    public Sprite[] midBackgroundSprites;
    public Sprite[] backgroundSprites;

    [SerializeField] GameObject ceilingPrefab;
    [SerializeField] GameObject floorPrefab;
    [SerializeField] GameObject stalactitePrefab;
    [SerializeField] GameObject midBackgroundPrefab;
    [SerializeField] GameObject backgroundPrefab;
    [SerializeField] GameObject ceilingStalactitePrefab;

    GameObject backgroundComponent;
    GameObject midBackgroundComponent;

    [SerializeField] private Queue<GameObject> ceilingList;
    [SerializeField] private Queue<GameObject> backgroundList;
    [SerializeField] private Queue<GameObject> midBackgroundList;

    GameObject newCeiling;
    GameObject newFloor;
    GameObject ceiling;
    GameObject newMidBg;
    GameObject newBg;

    public float timeToIncreaseVelocity;
    float velocityTimeElapsed;

    void Start()
    {
        backgroundComponent = FindObjectOfType<BackgroundBehaviour>().gameObject;
        midBackgroundComponent = FindObjectOfType<MidBackgroundBehaviour>().gameObject;

        ceilingList = new Queue<GameObject>();
        backgroundList = new Queue<GameObject>();
        midBackgroundList = new Queue<GameObject>();

        var rand = new System.Random();

        for (int i = 0; i < 20; i++)
        {
            newCeiling = Instantiate(ceilingPrefab, new Vector2(this.transform.position.x - 19 + i, 2.8f), Quaternion.identity, this.transform);
            newCeiling.gameObject.GetComponent<SpriteRenderer>().sprite = ceilingSprites[rand.Next(0, 4)];
            newCeiling.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);
            ceilingList.Enqueue(newCeiling);

            ceiling = Instantiate(ceilingStalactitePrefab, new Vector3(newCeiling.transform.position.x, 2.8f, 8), Quaternion.identity, newCeiling.transform);
            ceiling.gameObject.GetComponent<SpriteRenderer>().sprite = stalactiteSprites[rand.Next(0, 4)];
            ceiling.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);

            newFloor = Instantiate(ceilingPrefab, new Vector2(this.transform.position.x - 19 + i, -3), Quaternion.identity, this.transform);
            newFloor.gameObject.GetComponent<SpriteRenderer>().sprite = floorSprites[rand.Next(0, 4)];
            newFloor.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);
            ceilingList.Enqueue(newFloor);

            backgroundComponent.transform.position = new Vector3(this.transform.position.x, -3, 10);

            newBg = Instantiate(backgroundPrefab, new Vector3(this.transform.position.x - 19 + i, -1.9f, 10), Quaternion.identity, backgroundComponent.transform);
            newBg.gameObject.GetComponent<SpriteRenderer>().sprite = backgroundSprites[rand.Next(0, 4)];
            newBg.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel/3, 0);
            backgroundList.Enqueue(newBg);

            newMidBg = Instantiate(midBackgroundPrefab, new Vector3(this.transform.position.x - 19 + i, -2.2f, 5), Quaternion.identity, midBackgroundComponent.transform);
            newMidBg.gameObject.GetComponent<SpriteRenderer>().sprite = midBackgroundSprites[rand.Next(0, 4)];
            newMidBg.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel/2, 0);
            midBackgroundList.Enqueue(newMidBg);
        }

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

            newCeiling = Instantiate(ceilingPrefab, new Vector2(newCeiling.transform.position.x + 1, 2.8f), Quaternion.identity, this.transform);
            newCeiling.gameObject.GetComponent<SpriteRenderer>().sprite = ceilingSprites[rand.Next(0,4)];
            newCeiling.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);
            ceilingList.Enqueue(newCeiling);

            newFloor = Instantiate(floorPrefab, new Vector2(newFloor.transform.position.x + 1, -3), Quaternion.identity, this.transform);
            newFloor.gameObject.GetComponent<SpriteRenderer>().sprite = floorSprites[rand.Next(0,4)];
            newFloor.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);
            ceilingList.Enqueue(newFloor);

            tryToCreateStalactite();
            tryToCreateCeiling();

            if(ceilingList.Count >= 40) { 
                Destroy(ceilingList.Dequeue());
                Destroy(ceilingList.Dequeue());
            }
        }

        if (newBg.transform.position.x + 1 <= this.transform.position.x)
        {
            var rand = new System.Random();

            newBg = Instantiate(backgroundPrefab, new Vector3(newBg.transform.position.x + 1, newBg.transform.position.y, 10), Quaternion.identity, backgroundComponent.transform);
            newBg.gameObject.GetComponent<SpriteRenderer>().sprite = backgroundSprites[rand.Next(0, 4)];
            newBg.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel / 3, 0);
            backgroundList.Enqueue(newBg);

            if (backgroundList.Count >= 20)
            {
                Destroy(backgroundList.Dequeue());
            }
        }

        if (newMidBg.transform.position.x + 1 <= this.transform.position.x)
        {
            var rand = new System.Random();

            newMidBg = Instantiate(midBackgroundPrefab, new Vector3(newMidBg.transform.position.x + 1, newMidBg.transform.position.y, 5), Quaternion.identity, midBackgroundComponent.transform);
            newMidBg.gameObject.GetComponent<SpriteRenderer>().sprite = midBackgroundSprites[rand.Next(0, 4)];
            newMidBg.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel / 2, 0);
            midBackgroundList.Enqueue(newMidBg);

            if (midBackgroundList.Count >= 20)
            {
                Destroy(midBackgroundList.Dequeue());
            }
        }

        updateVelocity();
    }

    void tryToCreateStalactite()
    {
        if (UnityEngine.Random.value < randValue) 
        {
            var stalactite = Instantiate(stalactitePrefab, new Vector3(newCeiling.transform.position.x, 2.8f, 4), Quaternion.identity, newCeiling.transform);
            stalactite.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel , 0 );
        }
    }

    void tryToCreateCeiling()
    {
        var rand = new System.Random();

        ceiling = Instantiate(ceilingStalactitePrefab, new Vector3(newCeiling.transform.position.x, 2.8f, 8), Quaternion.identity, newCeiling.transform);
        ceiling.gameObject.GetComponent<SpriteRenderer>().sprite = stalactiteSprites[rand.Next(0,4)];
        ceiling.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel, 0);
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
            foreach(GameObject go in backgroundList)
            {
                go.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel/3, 0);
            }
            foreach (GameObject go in midBackgroundList)
            {
                go.GetComponent<Rigidbody2D>().velocity = new Vector2(-vel/2, 0);
            }

            velocityTimeElapsed = Time.time;
        }
    }

}
