using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class HealthPickupSpawnerBehaviour : MonoBehaviour
{

    [SerializeField] private GameObject healthPickupPrefab;
    [SerializeField] private float minWaitTime = 4f;
    [SerializeField] private float maxWaitTime = 10f;
    private SubDamageAndTimeBehaviour player;
    private bool hasSetSpawn = false;
    
    void Start()
    {
        player = FindObjectOfType<SubDamageAndTimeBehaviour>();
    }

    void Update()
    {
        if (player.Health < player.MaxHealth && !hasSetSpawn)
        {
            StartCoroutine(SpawnHealthPickup());
            hasSetSpawn = true;
        }
    }

    public void DisableSetSpawn()
    {
        hasSetSpawn = false;
    }

    private IEnumerator SpawnHealthPickup()
    {
        float timeToSpawn = Random.Range(minWaitTime, maxWaitTime);

        yield return new WaitForSeconds(timeToSpawn);

        float newX = Random.Range(-0.5f, 0.5f);
        float newY = Random.Range(-1.5f, 1.5f);

        Instantiate(healthPickupPrefab, new Vector3(newX, newY, -1) + this.transform.position, Quaternion.identity, this.transform);
    }
    
}
