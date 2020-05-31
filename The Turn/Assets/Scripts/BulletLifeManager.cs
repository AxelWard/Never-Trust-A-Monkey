using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BulletLifeManager : MonoBehaviour
{
    public GameObject particles;

    float initTime;
    PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        initTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(!checkLife())
        {
            blowUp();
        }
    }

    bool checkLife()
    {
        if(Time.time > initTime + 5)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SetPlayerController(PlayerController newPC)
    {
        pc = newPC;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(gameObject.tag == "PlayerBullet" && pc != null)
        {
            if(collision.gameObject.tag == "Unfriendly")
            {
                pc.scoreAdd += 1;
            }
        }

        blowUp();
    }

    private void blowUp()
    {
        Material bulletMaterial = gameObject.GetComponent<Renderer>().material;
        Vector3 velocity = gameObject.GetComponent<Rigidbody>().velocity;
        AudioClip clip = GetComponent<AudioSource>().clip;
        Destroy(gameObject);

        int count = Random.Range(3, 5);
        for(int i = 0; i < count; i++)
        {
            GameObject particle = Instantiate(particles, gameObject.transform.position, gameObject.transform.rotation);
            particle.GetComponent<Renderer>().material = bulletMaterial;
            particle.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(10f, 15f), Random.Range(-1.0f, 1.0f)) * Random.Range(1.0f, 5.0f) + velocity);
            particle.GetComponent<ParticleLifeManager>().makeParticle();
            if(i == 0)
            {
                particle.GetComponent<AudioSource>().clip = clip;
                particle.GetComponent<AudioSource>().Play();
            }
        }
    }
}
