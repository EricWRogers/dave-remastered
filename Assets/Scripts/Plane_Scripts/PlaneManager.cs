using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class PlaneManager : MonoBehaviour   //general home for all overarching functions of the plane and all public variables
{
    [Header("General Values")]
    [Tooltip("The amount of points gained for destroying this enemy")] public int points = 100;
    [Tooltip("The amount of health restored to the player")] public float healthRestored = 15f;
    [Tooltip("The overall speed of the plane")] public float speed = .5f;
    [Tooltip("The height from the center of the player that the plane should aim for")] public float desiredHeight = 10f;
    [Tooltip("The amount of flight time of the plane after passing over the player")] public float flyTime = 3f;
    [Tooltip("Should the plane lock it's vertical movement when passing over the player")] public bool straightMovement = true;

    [Header("Gun Values")]
    [Tooltip("The range from which the gun can 'see' the player")] public float gunRange = 60f;
    [Tooltip("The amount of damage each bullet does")] public float damage = 10f;
    [Tooltip("The turn speed at which the gun rotates")] public float gunTurnSpeed = 1f;
    [Tooltip("The amount of time between each shot of the gatling gun")] public float bulletInterval = .5f;

    [Header("Public Objects & Particles")]
    [Tooltip("The gun GameObject")] public GameObject gun;
    [Tooltip("The main menu used to check whether AI should start")] public MainMenu menu;
    [Tooltip("The smoke particles that appear on and persist after death")] public GameObject deathSmoke;
    [Tooltip("The explosion effect that is played on death")] public GameObject explosion;
    [Tooltip("The radius at which the smoke particles will not be enabled")] public float deathSmokeTriggerRadius;
    public GameObject flash;
    public AudioClip deathSound;

    [HideInInspector] public bool secondTime = false;
    private GameObject player;
    [HideInInspector]
    public Rigidbody rb;
    private Animator anim;
    public XRGrabInteractable grabbable;
    private Collider[] planeColliders;
    public LookAtPlayer look;
    [HideInInspector]
    public float Distance;
    [HideInInspector]
    public Quaternion lastRotation;
    private GameManager gManager;
    private bool smokeSpawned = false;
    private GameObject smoke;
    private bool isDead;
    [HideInInspector]
    public float runningTime = 0;
    public AudioSource atkSound;
    public AudioSource moveNoise;
    [HideInInspector]
    public bool startedSound = false;
    private bool triggered = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        planeColliders = GetComponents<Collider>();
        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        moveNoise = transform.parent.GetComponent<AudioSource>();

        moveNoise.Play();

        grabbable.enabled = false;
    }

    private void LateUpdate()
    {
        if (!anim.enabled)
        {
            transform.parent.Translate(transform.forward * speed * Time.deltaTime, Space.World);
            runningTime += Time.deltaTime;

            if (runningTime >= flyTime)
            {
                anim.enabled = true;
                anim.SetTrigger("Turn");
            }
        }

        Vector3 finishPoint = new Vector3(player.transform.position.x, player.transform.position.y + desiredHeight, player.transform.position.z);
        Distance = Vector3.Distance(finishPoint, transform.position);


        if (isDead)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, deathSmokeTriggerRadius, 383);
            bool hitPlayer = false;
            if (hits.Length > 0)
            {
                if (hits[0].tag == "PlayerGround")
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

    void FireGun()  //called in RepeatFireGun()
    {
        GameObject flashFX = Instantiate(flash, gun.transform.position, gun.transform.rotation);
        flashFX.transform.localScale = new Vector3(.01f, .01f, .01f);
        flashFX.transform.parent = gun.transform;
        Destroy(flashFX, bulletInterval);

        if (!startedSound)
        {
            atkSound.Play();
            startedSound = true;
        }

        RaycastHit hit;
        if (Physics.Raycast(gun.transform.position, gun.transform.forward, out hit, gunRange))
        {
            Debug.DrawRay(gun.transform.position, gun.transform.forward * gunRange, Color.red, .5f);
            if (hit.transform.GetComponent<Health>() != null)
                hit.transform.GetComponent<Health>().TakeDamage(damage);
        }
    }

    public void TurnOffLook()
    {
        look.enabled = false;
    }

    public void TurnOnLook()
    {
        look.enabled = true;
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
                }

                if (!smokeSpawned)
                {
                    GameObject explodeSmoke = Instantiate(explosion, transform.position, transform.rotation);
                    Destroy(explodeSmoke, 1f);
                    smoke = Instantiate(deathSmoke, transform.position, Quaternion.identity);
                    smoke.transform.parent = transform;
                    Quaternion rotation = Quaternion.Euler(-47.9f, 81.1f, 54.8f);
                    smoke.transform.rotation = rotation;
                    smoke.transform.localScale = new Vector3(.1f, .1f, .1f);
                    smokeSpawned = true;
                    smoke.SetActive(true);
                    smoke.GetComponent<ParticleSystem>().Play();
                    isDead = true;
                }

                look.enabled = false;
                anim.SetTrigger("Died");
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.None;
                anim.enabled = false;
                grabbable.enabled = true;
                gManager.GetComponent<PointManager>().score += points;

                transform.parent.GetComponent<AudioSource>().Stop();
                transform.parent.GetComponent<AudioSource>().loop = false;
                transform.parent.GetComponent<AudioSource>().PlayOneShot(deathSound, .2f);
                GetComponent<AudioSource>().Stop();

                foreach (Collider collider in planeColliders)
                {
                    collider.isTrigger = false;
                }

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
                    GameObject explodeSmoke = Instantiate(explosion, transform.position, transform.rotation);
                    Destroy(explodeSmoke, 1f);
                    smoke = Instantiate(deathSmoke, transform.position, Quaternion.identity);
                    smoke.transform.parent = transform;
                    Quaternion rotation = Quaternion.Euler(-47.9f, 81.1f, 54.8f);
                    smoke.transform.rotation = rotation;
                    smoke.transform.localScale = new Vector3(.1f, .1f, .1f);
                    smokeSpawned = true;
                    smoke.SetActive(true);
                    smoke.GetComponent<ParticleSystem>().Play();
                    isDead = true;
                }

                look.enabled = false;
                anim.SetTrigger("Died");
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.None;
                anim.enabled = false;
                grabbable.enabled = true;
                gManager.GetComponent<PointManager>().score += points;

                transform.parent.GetComponent<AudioSource>().Stop();
                transform.parent.GetComponent<AudioSource>().loop = false;
                transform.parent.GetComponent<AudioSource>().PlayOneShot(deathSound, .2f);
                GetComponent<AudioSource>().Stop();

                foreach (Collider collider in planeColliders)
                {
                    collider.isTrigger = false;
                }

                triggered = true;
            }
        }
    }

    public void RepeatFireGun()     //called at the start of Plane_Track and cancelled on state exit
    {
        InvokeRepeating("FireGun", 1f, bulletInterval);
    }

    public void CancelGun()     //cancelled on state exit of Plane_Track
    {
        CancelInvoke("FireGun");
    }

    public void RotateGun()     //constantly rotates the gun to face the player. on state update of Plane_Track
    {
        Vector3 targetPoint = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z) - gun.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetPoint, Vector3.up);
        gun.transform.rotation = Quaternion.Slerp(gun.transform.rotation, targetRotation, Time.deltaTime * gunTurnSpeed);
    }

    public void FlyPast()       //handles which direction to move in once directly above the player
    {
        if (!secondTime)
            transform.parent.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        if (secondTime)
            transform.parent.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    public void StartFlyTimer()
    {
        StartCoroutine(FlyTimer());
    }

    IEnumerator FlyTimer()
    {
        yield return new WaitForSeconds(flyTime);
        anim.SetTrigger("Turn");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;         //draws the orbit range in green in the editor
        Gizmos.DrawWireSphere(transform.position, deathSmokeTriggerRadius);

        Gizmos.color = Color.red;         //draws the orbit range in green in the editor
        Gizmos.DrawRay(gun.transform.position, gun.transform.forward * gunRange);
    }
}
