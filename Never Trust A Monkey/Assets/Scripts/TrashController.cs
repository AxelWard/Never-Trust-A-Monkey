using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : MonoBehaviour
{
    private TrashSpawner ts;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerController>().scoreAdd += 5;
            ts.TriggerPickup();
            Destroy(gameObject);
        }
    }

    public void SetSpawner(TrashSpawner newTS)
    {
        ts = newTS;
    }
}
