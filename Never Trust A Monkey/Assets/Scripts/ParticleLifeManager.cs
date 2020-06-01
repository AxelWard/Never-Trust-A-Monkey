using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLifeManager : MonoBehaviour
{
    float initTime;
    float lifeTime;

    // Start is called before the first frame update
    public void makeParticle()
    {
        initTime = Time.time;
        lifeTime = Random.Range(0.25f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!checkLife())
        {
            Destroy(gameObject);
        }
    }

    bool checkLife()
    {
        if (Time.time > initTime + lifeTime)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
