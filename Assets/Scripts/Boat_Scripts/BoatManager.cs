using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BoatManager : MonoBehaviour
{
    [Tooltip("The amount of points gained for destroying this enemy")] public int points = 100;
    [Tooltip("The amount of health restored to the player")] public float healthRestored = 15f;
    public float attackRange = 40f;
    public float gunTurnSpeed = 1f;
    public float gunRange = 50f;
    public float damage = 5f;
    public float bulletInterval = 1f;
    public GameObject gun;
    public Collider modelCollider;
    public GameObject flash;
    public AudioSource atkSound;
    public AudioSource boatSoundSource;
    [Tooltip("The smoke particles that appear on and persist after death")] public GameObject deathSmoke;
    [Tooltip("The explosion effect that is played on death")] public GameObject explosion;
    [Tooltip("The radius at which the smoke particles will not be enabled")] public float deathSmokeTriggerRadius;
    public AudioClip deathSound;
    public ParticleSystem[] waterEffects;

    private float Distance;
    private GameObject player;
    public XRGrabInteractable grabbable;
    private Rigidbody rb;
    private Animator anim;
    private GameManager gManager;
    private bool smokeSpawned = false;
    private GameObject smoke;
    private bool isDead = false;
    private bool triggered = false;
    [HideInInspector]
    public bool startedSound = false;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerGround");
        rb = GetComponent<Rigidbody>();
        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        anim = GetComponent<Animator>();

        grabbable.enabled = false;
    }

    void Update()
    {
        Distance = Vector3.Distance(player.transform.position, transform.parent.position);

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

    void FireGun()
    {
        GameObject flashFX = Instantiate(flash, gun.transform.Find("BarrelTip").position, gun.transform.rotation);
        flashFX.transform.localScale = new Vector3(.3f, .3f, .3f);
        flashFX.transform.parent = gun.transform;
        Destroy(flashFX, bulletInterval);

        RaycastHit hit;
        if (Physics.Raycast(gun.transform.position, gun.transform.forward, out hit, gunRange))
        {
            Debug.Log(hit.transform.tag);
            Debug.DrawRay(gun.transform.position, gun.transform.forward * gunRange, Color.red, bulletInterval);
            if (hit.transform.GetComponent<Health>() != null)
                hit.transform.GetComponent<Health>().TakeDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            if (other.tag == "Player" || other.tag == "PlayerProjectile")
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
                    Quaternion rotation = Quaternion.Euler(-47.9f, 81.1f, 54.8f);
                    smoke.transform.rotation = rotation;
                    smoke.transform.localScale = new Vector3(.3f, .3f, .3f);
                    smokeSpawned = true;
                    smoke.SetActive(true);
                    smoke.GetComponent<ParticleSystem>().Play();
                    isDead = true;
                }

                modelCollider.enabled = true;
                grabbable.enabled = true;
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.None;
                anim.SetTrigger("Dead");
                Destroy(GetComponentInParent<UnityEngine.AI.NavMeshAgent>());
                gManager.GetComponent<PointManager>().score += points;
                CancelInvoke();

                transform.parent.GetComponent<AudioSource>().Stop();
                transform.parent.GetComponent<AudioSource>().loop = false;
                transform.parent.GetComponent<AudioSource>().clip = deathSound;
                transform.parent.GetComponent<AudioSource>().Play();
                GetComponent<AudioSource>().Stop();

                triggered = true;
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!triggered)
        {
            if (other.tag == "Player" || other.tag == "PlayerProjectile" || other.tag == "FireBreath" || other.tag == "Debris")
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
                    Quaternion rotation = Quaternion.Euler(-47.9f, 81.1f, 54.8f);
                    smoke.transform.rotation = rotation;
                    smoke.transform.localScale = new Vector3(.3f, .3f, .3f);
                    smokeSpawned = true;
                    smoke.SetActive(true);
                    smoke.GetComponent<ParticleSystem>().Play();
                    isDead = true;
                }

                modelCollider.enabled = true;
                grabbable.enabled = true;
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.None;
                anim.SetTrigger("Dead");
                Destroy(GetComponentInParent<UnityEngine.AI.NavMeshAgent>());
                gManager.GetComponent<PointManager>().score += points;
                CancelInvoke();

                transform.parent.GetComponent<AudioSource>().Stop();
                transform.parent.GetComponent<AudioSource>().loop = false;
                transform.parent.GetComponent<AudioSource>().clip = deathSound;
                transform.parent.GetComponent<AudioSource>().Play();
                GetComponent<AudioSource>().Stop();

                triggered = true;
            }
        }
    }

    public void RepeatFireGun()
    {
        InvokeRepeating("FireGun", 1f, bulletInterval);
    }

    public void CancelRepeat()
    {
        CancelInvoke("FireGun");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;         //draws the attackRange in green in the editor
        Gizmos.DrawWireSphere(transform.position, gunRange);

        Gizmos.color = Color.cyan;         //draws the attackRange in green in the editor
        Gizmos.DrawWireSphere(transform.position, deathSmokeTriggerRadius);
    }
}
