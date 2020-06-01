using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingScript : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Unfriendly" || other.gameObject.tag == "Friendly")
        {
            Rigidbody playerRigidBody = other.GetComponent<Rigidbody>();
            playerRigidBody.velocity = new Vector3(playerRigidBody.velocity.x, 0f, playerRigidBody.velocity.z);
        }
    }
}
