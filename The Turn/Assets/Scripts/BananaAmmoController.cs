using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaAmmoController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<PlayerController>().RefillAmmo();
            Destroy(gameObject);
        }
    }
}
