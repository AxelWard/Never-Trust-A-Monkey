using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public int trashLimit;
    public int trashCount;
    public float range;
    public float spawnInterval;

    public GameObject[] trash;

    private float lastSpawnTime = 0f;

    private void Update()
    {
        if (Time.time > lastSpawnTime + spawnInterval)
        {
            spawnTrash();
            lastSpawnTime = Time.time;
        }
    }

    private void spawnTrash()
    {
        if (trashCount < trashLimit)
        {
            float spawnX = Random.Range(0f, range) - (range / 2) + gameObject.transform.position.x;
            float spawnZ = Random.Range(0f, range) - (range / 2) + gameObject.transform.position.z;

            int spawn = Random.Range(0, trash.Length);
            GameObject spawned = Instantiate(trash[spawn], new Vector3(spawnX, 2.1f, spawnZ), new Quaternion(0f, 0f, 0f, 0f));
            spawned.GetComponent<TrashController>().SetSpawner(this);
            spawned.transform.Rotate(new Vector3(Random.Range(-90f, 90f), Random.Range(-90f, 90f), Random.Range(-90f, 90f)));

            trashCount++;
        }
    }

    public void TriggerPickup()
    {
        gameObject.GetComponent<AudioSource>().Play();
        trashCount--;
    }
}
