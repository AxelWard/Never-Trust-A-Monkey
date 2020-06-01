using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject target;
    public float speed;
    public float speedLimit;
    public float aggroChance;
    public float randomRunThreshold;

    Transform enemyTrans;
    Rigidbody enemyRigidBody;
    GunController gun;
    Animator anim;

    private float lastTargetCheck;
    bool activeEnemy;
    private float randomTurnX;
    private float randomTurnZ;
    private float lastRandomizeTime;
    private float nextRandomizeTime;
    private float runTime;
    private bool runForward;

    private void Start()
    {
        enemyTrans = GetComponent<Transform>();
        enemyRigidBody = GetComponent<Rigidbody>();
        gun = GetComponentInChildren<GunController>();
        anim = GetComponent<Animator>();

        lastTargetCheck = Time.time + 5f;
        lastRandomizeTime = Time.time + Random.Range(0f, 10f);
        target = null;
        runForward = false;
        target = getTarget();
        activeEnemy = false;
    }

    private void Update()
    {
        if (Time.time > lastRandomizeTime + runTime)
        {
            runForward = false;
            anim.SetBool("Target", false);
        }
        else
        {
            anim.SetFloat("Run Multiplier", 1.6f);
        }

        if (activeEnemy && !PlayerController.PLAYERISDEAD)
        {
            if (getTargetDistance() < 8)
            {
                anim.SetBool("Target", true);
                anim.SetFloat("Run Multiplier", 0f);
                getTargetHeight();
                gun.show();
                runForward = false;
                turnToFaceTarget();

                if(closeToTarget())
                {
                    gun.setThrowing();
                    gun.triggerDown();
                }
                else
                {
                    gun.triggerUp();
                }
            }
            else
            {
                turnToRandomDirection();
                gun.hide();
            }
        } 
        else
        {
            gun.hide();
            turnToRandomDirection();
            
            if(Time.time > lastTargetCheck + .5f)
            {
                rollForAggro();
                lastTargetCheck = Time.time;
            }
        }
    }

    private void FixedUpdate()
    {
        if(enemyRigidBody.velocity.magnitude < speedLimit && runForward)
        {
            enemyRigidBody.AddForce(enemyTrans.TransformDirection(Vector3.forward) * speed);
        }
    }

    private bool closeToTarget()
    {
        if (target != null) {
            if (getTargetDistance() < 5)
            {
                anim.SetFloat("Run Multiplier", 0.0f);
                return true;
            }
        }

        return false;
    }

    private void turnToFaceTarget()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.y = gameObject.transform.position.y;

            transform.LookAt(targetPosition);
        }
    }

    private void turnToRandomDirection()
    {
        Vector3 targetPosition = gameObject.transform.position;

        targetPosition.x += randomTurnX;
        targetPosition.z += randomTurnZ;

        if (Time.time > lastRandomizeTime + nextRandomizeTime)
        {
            randomTurnX = Random.Range(-randomRunThreshold, randomRunThreshold);
            randomTurnZ = Random.Range(-randomRunThreshold, randomRunThreshold);
            lastRandomizeTime = Time.time;
            nextRandomizeTime = Random.Range(0.5f, 2.0f);
            runTime = Random.Range(0.5f, 1.0f);
            runForward = true;

            transform.LookAt(targetPosition);
            anim.SetBool("Target", true);
        }
    }

    private float getTargetDistance()
    {
        float distance = 0.0f;
        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;
            distance = (enemyTrans.position - targetPosition).magnitude;
        }

        return distance;
    }

    private GameObject getTarget()
    {
        GameObject[] objectArray = FindObjectsOfType<GameObject>();
        List<GameObject> friendlyList = new List<GameObject>();

        foreach(GameObject potential in objectArray)
        {
            if(potential.tag == "Player")
            {
                friendlyList.Add(potential);
            }
        }

        int index = -1;
        int i = 0;
        float closest = 100000000;
        foreach(GameObject isClosest in friendlyList)
        { 
            float distance = (isClosest.transform.position - gameObject.transform.position).magnitude;
            if(distance < closest)
            {
                closest = distance;
                index = i;
            }
            i++;
        }

        if (index > -1)
        {
            return friendlyList[index];
        }
        else
        {
            return null;
        }
    }

    private void rollForAggro()
    {
        float roll = Random.Range(0.0f, 100f);
        if(roll > 100f - aggroChance)
        {
            activeEnemy = true;
            gameObject.tag = "Unfriendly";
            getTarget();
        }
    }

    private void getTargetHeight()
    {
        if(target != null)
        {
            float heightDiff = enemyTrans.position.y - target.transform.position.y;
            float distance = getTargetDistance();

            float angle = Mathf.Asin(heightDiff / distance) * Mathf.Rad2Deg;

            float percent = ((angle + 25) / 50);

            gun.setPitch(Mathf.Clamp(percent, 0, 1));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "PlayerBullet")
        {
            activeEnemy = false;
            gameObject.tag = "Friendly";
            gun.hide();
            gun.triggerUp();
            lastTargetCheck = Time.time + 2f;
        }
        else if(collision.gameObject.tag == "EnemyBullet")
        {
            activeEnemy = true;
            gameObject.tag = "Unfriendly";
        }
    }
}
