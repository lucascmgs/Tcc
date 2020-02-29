using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CeilingBehaviour : MonoBehaviour
{

    private float timeElapsed;
    private bool createCeiling;
    public float setTime;
    public float randValue;

    public Sprite [] sprites;

    [SerializeField] GameObject ceilingPrefab;

    [SerializeField] GameObject stalactitePrefab;

    void Start()
    {
        timeElapsed = 0;
        createCeiling = true;
    }

    void Update()
    {
        if (createCeiling) {
            var newCeiling = Instantiate(ceilingPrefab, this.transform.position, Quaternion.identity);
            var newValue = UnityEngine.Random.value;

            if (newValue >= 0 && newValue <= 0.6)
            {
                newCeiling.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[0];
                if (UnityEngine.Random.value > randValue)
                {
                    var newStalactite = Instantiate(stalactitePrefab, this.transform.position, Quaternion.identity);
                }
            }

            else if (newValue > 0.6 && newValue <= 0.8) newCeiling.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
            
            else newCeiling.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[2];

            createCeiling = false;
            timeElapsed = Time.time;
        }
        else
        {
            if(Time.time - timeElapsed >= setTime) { createCeiling = true; }
        }
    }
}
