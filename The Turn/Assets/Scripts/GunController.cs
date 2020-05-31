﻿using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bullet;
    public float fireStrength;
    public float reloadInterval;

    float pitch;
    float lastFired;

    bool fireCheck;
    bool throwing;

    PlayerController pc;

    private void Start()
    {
        lastFired = -1;
        throwing = false;
    }

    public void triggerDown()
    {
        fireCheck = true;
    }

    public void triggerUp()
    {
        fireCheck = false;
    }    

    public void setThrowing()
    {
        throwing = true;
    }

    public void SetPlayerController(PlayerController newPC)
    {
        pc = newPC;
    }

    public void show()
    {
        gameObject.SetActive(true);
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (lastFired < Time.fixedTime - reloadInterval && fireCheck == true)
        {
            Vector3 randomize;
            if(throwing)
            {
                randomize = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
            }
            else
            {
                randomize = new Vector3(0f, 0f, 0f);
            }

            lastFired = Time.fixedTime;
            GameObject fired = Instantiate(bullet, gameObject.transform.position + (gameObject.transform.forward * 1.25f), gameObject.transform.rotation);
            fired.GetComponent<Rigidbody>().AddForce((gameObject.transform.forward + randomize) * fireStrength);
            fired.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-15f, 15f), Random.Range(-15f, 15f), Random.Range(-15f, 15f));
            fired.GetComponent<BulletLifeManager>().SetPlayerController(pc);
            GetComponent<AudioSource>().Play();
        }
    }

    public void setPitch(float newPitch)
    {
        pitch = newPitch;
        gameObject.transform.localEulerAngles = new Vector3(-25 + 50 * pitch, 0f, 0f);
    }
}
