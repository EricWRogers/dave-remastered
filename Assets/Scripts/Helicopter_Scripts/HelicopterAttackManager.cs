using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterAttackManager : MonoBehaviour
{
    [Header("Values for tweaking")]
    [Tooltip("Damage of gatling gun shots. Missiles are handled in the projectile script")] public float damage = 10f;
    [Tooltip("Maximum range of the rays from the gatling gun")] public float range = 2000f;
    [Tooltip("Maximum duration of gatling gun")] public float duration = 10f;
    [Tooltip("The interval between every shot of the gatling gun")] public float gatlingInterval = .5f;
    [Tooltip("The interval between every missile")] public float missileInterval = 2f;
    public float attackRange = 5f;

    [Header("Public Objects")]
    [Tooltip("Holds muzzle flash effects")] public GameObject flash;
    [Tooltip("Holds the missile object")] public GameObject missile;
    [Tooltip("Holds the gun objects on the helicopter")] public GameObject[] guns;

    //counts the time the gatling gun has been firing
    private float fireTime = 0;
    private Transform player;
    private bool startedMissiles = false;
    private bool startedGatling = false;
    private bool startedSound = false;
    [HideInInspector]
    public AudioSource atkSound;

    void Start()
    {
        atkSound = GetComponent<AudioSource>();
        startedMissiles = false;
        startedGatling = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    void Update()
    {
        if (Vector3.Distance(transform.parent.position, player.position) <= attackRange)
        {
            if (!startedMissiles)
                InvokeRepeating("ShootMissile", 1f, missileInterval);
            if (!startedGatling)
                InvokeRepeating("GatlingGun", 1f, gatlingInterval);
            startedMissiles = true;
            startedGatling = true;
        }

        if (fireTime >= duration)
        {
            CancelInvoke("GatlingGun");
            fireTime = 0f;
            InvokeRepeating("GatlingGun", 5f, gatlingInterval);
            startedSound = false;
        }
    }

    void ShootMissile()
    {
        Instantiate(missile, guns[Random.Range(0, guns.Length)].transform.position, guns[Random.Range(0, guns.Length)].transform.rotation);
    }

    void GatlingGun()
    {
        GameObject randomGun = guns[Random.Range(0, guns.Length)];

        fireTime += Time.fixedDeltaTime;

        GameObject flashFX = Instantiate(flash, randomGun.transform.position, randomGun.transform.rotation);
        flashFX.transform.parent = randomGun.transform;
        Destroy(flashFX, gatlingInterval);

        if (!startedSound)
        {
            atkSound.Play();
            startedSound = true;
        }


        RaycastHit hit;
        if (Physics.Raycast(randomGun.transform.position, randomGun.transform.forward, out hit, range))
        {
            Debug.DrawRay(randomGun.transform.position, randomGun.transform.forward * range, Color.green, gatlingInterval);

            if (hit.transform.GetComponentInParent<Health>() != null)
                hit.transform.GetComponentInParent<Health>().TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.parent.position, transform.parent.forward * attackRange * -1f);
    }
}
