using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameOverChestBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject SparklePrefab;
    [SerializeField] private float rotation = 40;
    [SerializeField] private float maxTimeUntilNextSparkle = 0.4f;


    void Start()
    {
        StartCoroutine(CreateSparkles());
    }

    private IEnumerator CreateSparkles()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, this.rotation);
        while (true)
        {
            int amountToGenerate = (int) Random.Range(0, 3);
            for (int i = 0; i < amountToGenerate; i++)
            {
                float posX = Random.Range(-0.8f, 0.8f);
                float posy = Random.Range(-0.4f, -0.2f);

                var position = rotation * new Vector2(posX, posy);
                Instantiate(SparklePrefab, position, Quaternion.identity, this.transform);
            }

            yield return new WaitForSeconds(maxTimeUntilNextSparkle);
        }
    }

    
}