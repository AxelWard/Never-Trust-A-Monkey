using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float range;
    public float spawnInterval;
    public int spawnLimit;

    public GameObject enemy;

    private int numSpawned = 0;
    private float lastSpawnTime = 5f;

    private void Update()
    {
        if(Time.time > lastSpawnTime + spawnInterval)
        {
            spawnEnemy();
            lastSpawnTime = Time.time;
        }
    }

    private void spawnEnemy()
    {
        if(numSpawned < spawnLimit)
        {
            float spawnX = Random.Range(0f, range) - (range / 2) + gameObject.transform.position.x;
            float spawnZ = Random.Range(0f, range) - (range / 2) + gameObject.transform.position.z;

            Instantiate(enemy, new Vector3(spawnX, 2f, spawnZ), new Quaternion(0f, 0f, 0f, 0f));

            numSpawned++;
        }
    }
}
