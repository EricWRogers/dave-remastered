using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HelicopterManager : MonoBehaviour
{
    [Header("Orbiting Values")]
    [Tooltip("The range at which the helicopter stops tracking and orbits around the player")] public float orbitRange = 25f;
    [Tooltip("The max amount of time the helicopter can remain in place while orbiting")] public float maxOrbitTime = 5f;
    [Tooltip("The min amount of time the helicopter can remain in place while orbiting")] public float minOrbitTime = 2f;

    [Header("Flight Times")]
    [Tooltip("The min amount of time the helicopter can move in the upwards direction")] public float minTimeUp = 3f;
    [Tooltip("The max amount of time the helicopter can move in the upwards direction")] public float maxTimeUp = 3f;
    [Tooltip("The min amount of time the helicopter can move in the downwards direction")] public float minTimeDown = 3f;
    [Tooltip("The max amount of time the helicopter can move in the downwards direction")] public float maxTimeDown = 3f;
    [Tooltip("The min amount of time the helicopter can move in the left direction")] public float minTimeLeft = 3f;
    [Tooltip("The max amount of time the helicopter can move in the left direction")] public float maxTimeLeft = 3f;
    [Tooltip("The min amount of time the helicopter can move in the right direction")] public float minTimeRight = 3f;
    [Tooltip("The max amount of time the helicopter can move in the right direction")] public float maxTimeRight = 3f;
    
    [Header("Other")]
    [Tooltip("The height at which the helicopter will not choose to move up")] public float maxHeight = 50f;
    [Tooltip("The height at which the helicopter will not choose to move down")] public float minHeight = 25f;
    [Tooltip("The amount of points gained for destroying this enemy")] public int points = 100;
    [Tooltip("The amount of health restored to the player")] public float healthRestored = 15f;
    [Tooltip("The main menu used to check whether AI should start")] public MainMenu menu;

    [Header("Particles & Sounds")]
    [Tooltip("The smoke particles that appear on and persist after death")] public GameObject deathSmoke;
    [Tooltip("The explosion effect that is played on death")] public GameObject explosion;
    [Tooltip("The radius at which the smoke particles will not be enabled")] public float deathSmokeTriggerRadius;
    public AudioClip deathSound;
    public DespawnEnemy despawner;
    public Collider deadCollider;

    private Animator anim;
    private int Direction = -1;
    [HideInInspector]public bool started = false;
    private GameObject player;
    public XRGrabInteractable grabbable;
    private Rigidbody rb;
    private HelicopterAttackManager atkManager;
    private GameManager gManager;
    private bool isDead = false;
    private GameObject smoke;
    private bool smokeSpawned;
    private bool triggered = false;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        atkManager = GetComponent<HelicopterAttackManager>();
        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        despawner = GetComponent<DespawnEnemy>();
        grabbable.enabled = false;
    }

    void Update()
    {
        if (!isDead)
            PickDirection();


        if (!isDead)
            transform.parent.LookAt(2 * transform.parent.transform.position - player.transform.position);
     
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

    void PickDirection()
    {
        switch (Direction)
        {
            case 0:
                if (transform.position.y < maxHeight && started == false)
                {
                    anim.SetTrigger("Up");
                    started = true;
                }
                else Direction = Random.Range(0, 4);
                break;
            case 1:
                if (transform.position.y > minHeight && started == false)
                {
                    anim.SetTrigger("Down");
                    started = true;
                }
                else Direction = Random.Range(0, 4);
                break;
            case 2:
                if (started == false)
                {
                    anim.SetTrigger("Left");
                    started = true;
                }
                break;
            case 3:
                if (started == false)
                {
                    anim.SetTrigger("Right");
                    started = true;
                }
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            if (other.tag == "Player" || other.tag == "PlayerProjectile" || other.tag == "Debris" || other.tag == "Building")
            {
                Transform[] childTransforms = transform.parent.GetComponentsInChildren<Transform>();
                childTransforms[0] = transform.parent;
                foreach (Transform transform in childTransforms)
                {
                    transform.tag = "Debris";
                    transform.gameObject.layer = 12;
                }

                if (!smokeSpawned)
                {
                    GameObject explodeSmoke = Instantiate(explosion, transform.position, transform.parent.rotation);
                    Destroy(explodeSmoke, 1f);
                    smoke = Instantiate(deathSmoke, transform.position, Quaternion.identity);
                    smoke.transform.parent = transform;
                    smoke.transform.localScale = new Vector3(.1f, .1f, .1f);
                    Quaternion rotation = Quaternion.Euler(-47.9f, 81.1f, 54.8f);
                    smoke.transform.rotation = rotation;
                    smokeSpawned = true;
                    smoke.SetActive(true);
                    smoke.GetComponent<ParticleSystem>().Play();
                }

                isDead = true;
                despawner.StartShrink();
                deadCollider.enabled = true;
                grabbable.enabled = true;
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.None;
                anim.SetTrigger("Died");
                anim.enabled = false;
                gManager.GetComponent<PointManager>().score += points;
                atkManager.CancelInvoke();
                atkManager.enabled = false;

                GetComponent<AudioSource>().Stop();
                GetComponent<AudioSource>().clip = deathSound;
                GetComponent<AudioSource>().Play();
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
                }

                if (!smokeSpawned)
                {
                    GameObject explodeSmoke = Instantiate(explosion, transform.position, transform.parent.rotation);
                    Destroy(explodeSmoke, 1f);
                    smoke = Instantiate(deathSmoke, transform.position, Quaternion.identity);
                    smoke.transform.parent = transform;
                    smoke.transform.localScale = new Vector3(.1f, .1f, .1f);
                    Quaternion rotation = Quaternion.Euler(-47.9f, 81.1f, 54.8f);
                    smoke.transform.rotation = rotation;
                    smokeSpawned = true;
                    smoke.SetActive(true);
                    smoke.GetComponent<ParticleSystem>().Play();
                }

                isDead = true;
                grabbable.enabled = true;
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.None;
                rb.freezeRotation = false;
                anim.SetTrigger("Died");
                gManager.GetComponent<PointManager>().score += points;
                atkManager.CancelInvoke();
                atkManager.enabled = false;

                GetComponent<AudioSource>().Stop();
                GetComponent<AudioSource>().clip = deathSound;
                GetComponent<AudioSource>().Play();
                transform.parent.GetComponent<AudioSource>().Stop();

                triggered = true;
            }
        }
    }

    //called from the StateMachine
    public void StartOrbitTimer()
    {
        StartCoroutine(OrbitTimer());
    }

    public void StartMoveTimer(float minTime, float maxTime)
    {
        StartCoroutine(MoveTimer(minTime, maxTime));
    }

    //Waits for a random amount of time in orbit before picking a direction to move
    IEnumerator OrbitTimer()
    {
        yield return new WaitForSeconds(Random.Range(minOrbitTime, maxOrbitTime));
        Direction = Random.Range(0, 4);
    }

    IEnumerator MoveTimer(float minTime, float maxTime)
    {
        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        Direction = -1;
        anim.SetTrigger("FinishMoving");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;         //draws the orbit range in green in the editor
        Gizmos.DrawWireSphere(transform.position, orbitRange);

        Gizmos.color = Color.cyan;         //draws the orbit range in green in the editor
        Gizmos.DrawWireSphere(transform.position, deathSmokeTriggerRadius);
    }
}
