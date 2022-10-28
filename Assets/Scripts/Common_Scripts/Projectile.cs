using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Projectile : MonoBehaviour
{
    [Header("Values for tweaking")]
    [Tooltip("Projectile Speed")]
    public float speed = 70f;
    [Tooltip("Amount of damage each projectile does to the player")]
    public float damage = 10f;
    [Tooltip("The amount of time before the projectile is to be destroyed")]
    public float destroyTime = 20f;
    public List<AudioClip> clips;

    [HideInInspector]
    public bool explode = false;
    [HideInInspector]
    public float radius = 10f;
    [HideInInspector]
    public float force = 15f;
    [HideInInspector]
    public float lift = 10f;
    [HideInInspector]
    public bool explodeOnPlayer = false;

    [Header("Public for Unity")]
    [Tooltip("The particle effect used when the projectile hits the player")]
    public GameObject impactEffect;
    public GameObject launchEffect;
    public GameObject trailPosition;
    public AudioClip impactSound;
    public AudioClip launchingSound;

    private Transform target;
    private Rigidbody rb;
    public Explode explodeControl;
    private Transform child;
    private AudioSource audioSource;
    private GameObject launchFX;

    public void Start()
    {
        child = transform.Find("Missile Model Master");

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = launchingSound;
        audioSource.Play();

        launchFX = Instantiate(launchEffect, trailPosition.transform.position, trailPosition.transform.rotation);
        launchFX.transform.localScale = new Vector3(.05f, .05f, .05f);
        launchFX.transform.parent = gameObject.transform;

        target = GameObject.FindGameObjectWithTag("Player").transform;      //find our target, in this case the player
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);     //moves the projectile according to the moveDirection * speed
        Destroy(gameObject, destroyTime);     //starts the timer to destroy the projectile in the case it doesn't hit

        if (explode)
        {
            explodeControl = GetComponent<Explode>();
        }
    }

    void Update()
    {
        child.Rotate(new Vector3(0, 0, 10));

        if (target == null)     //if we have no target we destroy the projectile
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnTriggerEnter(Collider other)     //checks if the projectile has collided with the player
    {
        if (other.tag == "Player")
            HitTarget(other);
        if (other.tag != "Player" && other.tag != "Enemy" && other.tag != "Destroy")
        {
            audioSource.Stop();
            audioSource.clip = impactSound;
            audioSource.Play();

            if (explode)
                explodeControl.DoExplosion(radius, damage, force, lift);

            GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
            effectIns.transform.localScale = new Vector3(.4f, .4f, .4f);
            Destroy(effectIns, 1f);

            DestroyMissile();
        }

        if (other.tag == "Debris")
        {
            audioSource.Stop();
            audioSource.clip = impactSound;
            audioSource.Play();

            GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
            effectIns.transform.localScale = new Vector3(.4f, .4f, .4f);
            explodeControl.DoExplosion(radius, damage, force, lift);
            Destroy(effectIns, 1f);
            Destroy(other.gameObject);

            DestroyMissile();
        }
    }

    void HitTarget(Collider hit)
    {
        if (explodeOnPlayer)
            explodeControl.DoExplosion(radius, damage, force, lift);
        if (hit.GetComponent<Health>() != null)
            hit.GetComponent<Health>().TakeDamage(damage);      //adds the damage to the player

        audioSource.Stop();
        audioSource.clip = impactSound;
        audioSource.Play();

        GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        effectIns.transform.localScale = new Vector3(.4f, .4f, .4f);
        Destroy(effectIns, 1f);


        DestroyMissile();
    }

    void DestroyMissile()
    {
        rb.velocity = Vector3.zero;
        Destroy(launchFX);
        transform.GetChild(1).gameObject.SetActive(false);
        Destroy(gameObject, 3f);        //destroys the projectile when it hits the player
    }

    
}

#if UNITY_EDITOR
[CustomEditor(typeof(Projectile))]
public class RandomScript_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        Projectile script = (Projectile)target;

        // draw checkbox for the bool
        script.explode = EditorGUILayout.Toggle("Explode?", script.explode);
        if (script.explode) // if bool is true, show other fields
        {
            script.radius = EditorGUILayout.FloatField("Range", script.radius);
            script.force = EditorGUILayout.FloatField("Force", script.force);
            script.lift = EditorGUILayout.FloatField("Lift", script.lift);
            script.explodeOnPlayer = EditorGUILayout.Toggle("Explode on Player?", script.explodeOnPlayer);
        }
    }
}
#endif