using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject target;
    public float speed;
    public float speedLimit;
    public float aggroThreshold;
    public float aggroChance;
    public float randomRunThreshold;

    Transform enemyTrans;
    Rigidbody enemyRigidBody;
    GunController gun;
    Animator anim;

    private float lastTargetCheck;
    private bool activeEnemy;
    private float randomTurnX;
    private float randomTurnZ;
    private float lastRandomizeTime;

    private void Start()
    {
        enemyTrans = GetComponent<Transform>();
        enemyRigidBody = GetComponent<Rigidbody>();
        gun = GetComponentInChildren<GunController>();
        anim = GetComponent<Animator>();

        lastTargetCheck = -10f;
        lastRandomizeTime = -10f;
        target = null;
        activeEnemy = true;
    }

    private void Update()
    {
        if (activeEnemy)
        {
            turnToFaceTarget();
            getTargetHeight();

            if (getTargetDistance() > aggroThreshold)
            {
                target = null;
            }

            if (closeToTarget())
            {
                gun.triggerDown();
            }
            else
            {
                gun.triggerUp();
            }

            if (target != null)
            {
                if (Time.time > lastTargetCheck + 5f || target.gameObject.tag == "Unfriendly")
                {
                    target = getTarget();
                    lastTargetCheck = Time.time;
                }
            }
            else
            {
                if (Time.time > lastTargetCheck + 5f)
                {
                    target = getTarget();
                    lastTargetCheck = Time.time;
                }
            }
        } 
        else
        {
            if(Time.time > lastTargetCheck + 5f)
            {
                rollForAggro();
                lastTargetCheck = Time.time;
            }
        }

        if(target == null)
        {
            anim.SetBool("Target", false);
            gun.hide();
        }
        else
        {
            anim.SetBool("Target", true);
            gun.show();
        }

    }

    private void FixedUpdate()
    {
        if(enemyRigidBody.velocity.magnitude < speedLimit && !closeToTarget() && target != null)
        {
            enemyRigidBody.AddForce(enemyTrans.TransformDirection(Vector3.forward) * speed);
        } else if(target == null)
        {
            enemyRigidBody.velocity = new Vector3(0f, enemyRigidBody.velocity.y, 0f);
            enemyRigidBody.angularVelocity = new Vector3(0f, 0f, 0f);
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

        anim.SetFloat("Run Multiplier", 1.6f);
        return false;
    }

    private void turnToFaceTarget()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;
            targetPosition.y = gameObject.transform.position.y;

            if(getTargetDistance() > 7) {
                targetPosition.x += randomTurnX;
                targetPosition.z += randomTurnZ;
            }

            if (Time.time > lastRandomizeTime + 1.5f)
            {
                randomTurnX = Random.Range(-randomRunThreshold, randomRunThreshold);
                randomTurnZ = Random.Range(-randomRunThreshold, randomRunThreshold);
                lastRandomizeTime = Time.time;
            }

            transform.LookAt(targetPosition);
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
            if(potential.tag == "Friendly")
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
                if (distance < aggroThreshold)
                {
                    index = i;
                }
            }
            i++;
        }

        if (index > -1)
        {
            gun.setThrowing();
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
            target = null;
            gun.triggerUp();
        }
        else if(collision.gameObject.tag == "EnemyBullet")
        {
            activeEnemy = true;
            gameObject.tag = "Unfriendly";
        }
    }
}
