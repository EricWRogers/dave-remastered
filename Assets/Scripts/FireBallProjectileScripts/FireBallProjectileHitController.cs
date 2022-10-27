using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProjectileHitController : MonoBehaviour
{
    public GameObject particleEffect;

    void OnCollisionEnter(Collision collision)
    {
        // find contact point on object we collided with
        ContactPoint contact = collision.contacts[0];

        // rotate effect to angle of surface we contacted
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);

        // spawn particle effect at this position and rotation
        Instantiate(particleEffect, contact.point, rotation);

        // remove the projectile from the interactable game world.
        gameObject.SetActive(false);
    }
}
