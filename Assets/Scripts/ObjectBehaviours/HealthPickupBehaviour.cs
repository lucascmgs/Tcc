using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class HealthPickupBehaviour : MonoBehaviour
{
    private HealthPickupSpawnerBehaviour spawner;
    
    void Start()
    {
        spawner = FindObjectOfType<HealthPickupSpawnerBehaviour>();
    }

    private void OnDestroy()
    {
        spawner.DisableSetSpawn();
    }
}
