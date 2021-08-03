using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy should come from the hierarchy with XR Rig player set")]
    public GameObject enemy;
    public List<Transform> spawnLocations;
    public float respawnDelay = 2;
    public EnemyMonitor enemyMonitor;

    private float nextTime = 0;

    private void Update()
    {
        if (enemyMonitor.enemyCount < enemyMonitor.maxEnemies && Time.time > nextTime)
        {
            nextTime = Time.time + respawnDelay;
            Debug.Log("Spawning new enemy " + enemyMonitor.enemyCount);
            enemyMonitor.enemyCount++;
            System.Random rand = new System.Random();
            int location = rand.Next(0, spawnLocations.Count);
            Instantiate(enemy, spawnLocations[location].position, spawnLocations[location].rotation);
        }
    }

}