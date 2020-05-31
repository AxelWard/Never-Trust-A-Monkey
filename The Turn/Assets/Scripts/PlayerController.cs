using Cinemachine;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float speedLimit;
    public float jumpPower;
    public float sensitivity;
    public float knockbackPower;
    public float shakeIntensity;
    public float shakeTiming;
    public float stopMultiplier;
    public float maxPoop;
    public MaskController healthBarController;
    public int scoreAdd;
    public int totalScore;
    public AudioClip[] damageNoise;
    public int ammoLimit;

    Transform playerTrans;
    Rigidbody playerRigidBody;
    Animator anim;

    bool grounded;
    float pitch;
    GunController gun;
    float poop;
    public int ammo;
    float scoreTime;
    

    private void Start()
    {
        playerTrans = GetComponent<Transform>();
        playerRigidBody = GetComponent<Rigidbody>();
        gun = GetComponentInChildren<GunController>();
        anim = GetComponent<Animator>();

        grounded = false;
        poop = 0.0f;
        scoreAdd = 0;
        ammo = 0;

        gun.SetPlayerController(this);
    }

    private void Update()
    {
        if (!PauseMenuManager.GAMEISPAUSED)
        {
            if (Input.GetAxis("Mouse X") > 0)
            {
                playerTrans.Rotate(Vector3.up * sensitivity);
            }
            if (Input.GetAxis("Mouse X") < 0)
            {
                playerTrans.Rotate(Vector3.up * -sensitivity);
            }

            gun.setPitch(GetComponentInChildren<CinemachineFreeLook>().m_YAxis.Value);

            if (Input.GetMouseButtonDown(0))
            {
                gun.triggerDown();
            }
            if (Input.GetMouseButtonUp(0))
            {
                gun.triggerUp();
            }

            healthBarController.showPercentage = (poop / maxPoop) * 100;

            // scoreTime = Time.time;
            scoreTime = 0;
            totalScore = scoreAdd + (int)scoreTime;
        }
    }

    private void FixedUpdate()
    {
        // change to directional velocity
        Vector3 horizontalVel = playerRigidBody.velocity;
        horizontalVel.y = 0;
        if (horizontalVel.magnitude < speedLimit)
        {
            if (Input.GetKey(KeyCode.W))
            {
                playerRigidBody.AddForce(playerTrans.TransformDirection(Vector3.forward) * speed);
                anim.SetBool("Running", true);
                if(grounded) { anim.SetFloat("Run Speed", 1.5f); }
                // playerMesh.localEulerAngles = new Vector3(-89.98f, 0f, 0f);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                playerRigidBody.AddForce(-playerTrans.TransformDirection(Vector3.right) * speed * .5f);
                anim.SetBool("Running", true);
                if (grounded) { anim.SetFloat("Run Speed", -.75f); }
                // playerMesh.localEulerAngles = new Vector3(-89.98f, 40f, 0f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                playerRigidBody.AddForce(-playerTrans.TransformDirection(Vector3.forward) * speed * .75f);
                anim.SetBool("Running", true);
                if (grounded) { anim.SetFloat("Run Speed", -1.2f); }
                // playerMesh.localEulerAngles = new Vector3(-89.98f, 0f, 0f);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                playerRigidBody.AddForce(playerTrans.TransformDirection(Vector3.right) * speed * .5f);
                anim.SetBool("Running", true);
                if (grounded) { anim.SetFloat("Run Speed", .75f); }
                // playerMesh.localEulerAngles = new Vector3(-89.98f, 40f, 0f);
            } 
            else
            {
                // playerMesh.localEulerAngles = new Vector3(-89.98f, 0f, 0f);
                playerRigidBody.AddForce(-horizontalVel * stopMultiplier);
                anim.SetBool("Running", false);
            }
        }

        if(grounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                playerRigidBody.AddRelativeForce(playerTrans.up * jumpPower);
                grounded = false;
                anim.SetFloat("Run Speed", 0.0f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "EnemyBullet")
        {
            Vector3 knockBack = -playerTrans.TransformDirection(Vector3.forward) * 5;
            knockBack.y = 3f;
            playerRigidBody.AddForce(knockBack * knockbackPower);
            StartCoroutine(_ProcessShake());
            addPoop();
        }
        else
        {
            grounded = true;
        }
    }

    private void OnAnimatorMove()
    {
        
    }

    private IEnumerator _ProcessShake()
    {
        CinemachineFreeLook cam = GetComponentInChildren<CinemachineFreeLook>();
        setCamNoise(1, shakeIntensity, cam);
        yield return new WaitForSeconds(shakeTiming);
        setCamNoise(0, 0, cam);
    }

    public void setCamNoise(float amplitudeGain, float frequencyGain, CinemachineFreeLook cam)
    {
        cam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitudeGain;
        cam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequencyGain;

        cam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitudeGain;
        cam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequencyGain;

        cam.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitudeGain;
        cam.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequencyGain;
    }

    private void addPoop()
    {
        if(poop < maxPoop) {
            poop += Random.Range(7.5f, 12.5f);
        }
        else
        {
            FindObjectOfType<SceneSwitcher>().LoadNextScene();
        }

        int noise = Random.Range(0, damageNoise.Length - 1);
        GetComponent<AudioSource>().clip = damageNoise[noise];
        GetComponent<AudioSource>().Play();
    }

    public void RefillAmmo()
    {
        ammo = ammoLimit;
    }
}
