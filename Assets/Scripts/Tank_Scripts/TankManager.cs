using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TankManager : MonoBehaviour
{
    [Tooltip("The amount of health restored to the player")] public float healthRestored = 15f;
    [Tooltip("The amount of points gained for destroying this enemy")] public int points = 100;
    public float stoppingDistance = 25f;
    public GameObject gun;
    public float range = 10f;
    public float stopTime = 2f;
    public float waitTime = 2f;
    public float turretTurnSpeed = 1.5f;
    public GameObject missile;
    public float missileInterval = 2f;
    public GameObject barrelPivot;
    public GameObject topTurret;
    public GameObject tankModel;
    public GameObject deathSmoke;
    public GameObject explosion;
    public GameObject flash;
    public float deathSmokeTriggerRadius = 2f;
    public Collider[] modelColliders;
    public AudioClip deathSound;

    private Animator anim;
    private UnityEngine.AI.NavMeshAgent agent;
    private bool startedDestroy = false;
    private bool fired = false;
    private Rigidbody rb;
    public XRGrabInteractable grabbable;
    private GameObject player;
    private GameManager gManager;
    private bool isDead = false;
    private bool smokeSpawned = false;
    private GameObject smoke;
    private bool changedToTrigger = false;
    private bool triggered = false;
    [HideInInspector] public bool foundPlayer = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        agent = GetComponentInParent<UnityEngine.AI.NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        grabbable.enabled = false;
    }

    void Update()
    {
        HandleSmokeEffects();

        CheckDebris();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            if (other.tag == "Player" || other.tag == "PlayerProjectile" || other.tag == "Debris")
            {
                Transform[] childTransforms = transform.parent.GetComponentsInChildren<Transform>();
                childTransforms[0] = transform.parent;
                foreach (Transform transform in childTransforms)
                {
                    transform.tag = "Debris";
                    transform.gameObject.layer = 12;

                    if (transform.GetComponent<Collider>() != null && transform.GetComponent<Collider>().enabled == false)
                    {
                        transform.GetComponent<Collider>().enabled = true;
                    }
                }

                if (!smokeSpawned)
                {
                    GameObject explodeSmoke = Instantiate(explosion, transform.position, transform.rotation);
                    Destroy(explodeSmoke, 1f);
                    smoke = Instantiate(deathSmoke, transform.position, Quaternion.identity);
                    smoke.transform.parent = transform;
                    smoke.transform.localScale = new Vector3(.1f, .1f, .1f);
                    Quaternion rotation = Quaternion.Euler(-47.9f, 81.1f, 54.8f);
                    smoke.transform.rotation = rotation;
                    smokeSpawned = true;
                    smoke.SetActive(true);
                    smoke.GetComponent<ParticleSystem>().Play();
                    isDead = true;
                }

                anim.SetTrigger("Died");
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.None;
                grabbable.enabled = true;
                gManager.GetComponent<PointManager>().score += points;
                Destroy(agent);
                CancelInvoke();
                GetComponent<AudioSource>().PlayOneShot(deathSound);
                transform.parent.GetComponent<AudioSource>().Stop();

                triggered = true;
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!triggered)
        {
            if (other.tag == "Player" || other.tag == "PlayerProjectile" || other.tag == "Debris" || other.tag == "FireBreath")
            {
                Transform[] childTransforms = transform.parent.GetComponentsInChildren<Transform>();
                childTransforms[0] = transform.parent;
                foreach (Transform transform in childTransforms)
                {
                    transform.tag = "Debris";
                    transform.gameObject.layer = 12;

                    if (transform.GetComponent<Collider>() != null && transform.GetComponent<Collider>().enabled == false)
                    {
                        transform.GetComponent<Collider>().enabled = true;
                    }
                }

                if (!smokeSpawned)
                {
                    GameObject explodeSmoke = Instantiate(explosion, transform.position, transform.rotation);
                    Destroy(explodeSmoke, 1f);
                    smoke = Instantiate(deathSmoke, transform.position, Quaternion.identity);
                    smoke.transform.parent = transform;
                    smoke.transform.localScale = new Vector3(.1f, .1f, .1f);
                    Quaternion rotation = Quaternion.Euler(-47.9f, 81.1f, 54.8f);
                    smoke.transform.rotation = rotation;
                    smokeSpawned = true;
                    smoke.SetActive(true);
                    smoke.GetComponent<ParticleSystem>().Play();
                    isDead = true;
                }

                anim.SetTrigger("Died");
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.None;
                grabbable.enabled = true;
                gManager.GetComponent<PointManager>().score += points;
                Destroy(agent);
                CancelInvoke();
                GetComponent<AudioSource>().PlayOneShot(deathSound);
                transform.parent.GetComponent<AudioSource>().Stop();

                triggered = true;
            }
        }
    }

    void CheckDebris()
    {
        RaycastHit hit;
        if (Physics.Raycast(gun.transform.position, gun.transform.forward, out hit, range))
        {
            Debug.DrawRay(gun.transform.position, gun.transform.forward * range, Color.green);

            if (hit.transform.tag == "Debris")
            {
                if (!startedDestroy && !fired)
                {
                    StartCoroutine(DestroyObj(hit.transform.gameObject));
                    startedDestroy = true;
                    fired = true;
                }
            }

            if (hit.transform.tag == "Player")
            {
                foundPlayer = true;
            }
            else foundPlayer = false;
        }
    }

    private void HandleSmokeEffects()
    {
        if (isDead)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, deathSmokeTriggerRadius, 3328); //binary number: 00110100000000
            bool hitPlayer = false;
            if (hits.Length > 0)
            {
                if (hits[0].tag == "PlayerGround" || hits[0].tag == "Player")
                {
                    hitPlayer = true;
                }
            }
            if (!hitPlayer)
            {
                if (!smoke.GetComponent<ParticleSystem>().isEmitting)
                {
                    smoke.GetComponent<ParticleSystem>().Play();
                }

            }
            else
            {
                smoke.GetComponent<ParticleSystem>().Stop();
            }
        }
    }

    public void RotateTurret()
    {
        Vector3 targetPoint = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z) - gun.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetPoint, Vector3.up);
        topTurret.transform.rotation = Quaternion.Slerp(topTurret.transform.rotation, targetRotation, Time.deltaTime * turretTurnSpeed);
    }

    public void ShootMissile()
    {
        if (!isDead)
            Instantiate(missile, gun.transform.position, gun.transform.rotation);

        GameObject flashFX = Instantiate(flash, gun.transform.position, gun.transform.rotation);
        flashFX.transform.localScale = new Vector3(.01f, .01f, .01f);
        flashFX.transform.parent = gun.transform;
        Destroy(flashFX, missileInterval);

        GetComponent<AudioSource>().Play();
    }

    public void RepeatMissile()
    {
        InvokeRepeating("ShootMissile", 0f, missileInterval);
    }

    public void CancelMissile()
    {
        CancelInvoke("ShootMissile");
    }

    IEnumerator DestroyObj(GameObject obj)
    {
        if (agent != null)
            agent.isStopped = true;
        anim.speed = 0f;
        yield return new WaitForSeconds(stopTime);
        Instantiate(missile, gun.transform.position, gun.transform.rotation);
        yield return new WaitForSeconds(waitTime);
        agent.isStopped = false;
        anim.speed = 1f;
        startedDestroy = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;         //draws the stoppingRange range in green in the editor
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);

        Gizmos.color = Color.cyan;         //draws the stoppingRange range in green in the editor
        Gizmos.DrawWireSphere(transform.position, deathSmokeTriggerRadius);
    }
}
