using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : MonoBehaviour
{
    public GameObject currentHitObject;

    public float sphereRadius;
    public float maxDistance;
    public LayerMask layerMask;

    public float slowDuration;
    public float ccAccelerationDuringSlow = 0.05f;

    private Vector3 origin;
    private Vector3 direction;

    private float ccAccelerationDefault;
    public OVRPlayerController controller;
    private bool slowed = false;
    private float timer = 0f;

    private float currentHitDistance;

    void Start()
    {
        ccAccelerationDefault = controller.Acceleration;
    }

    void Update()
    {
        origin = transform.position;
        direction = -transform.up;
        RaycastHit hit;
        if (Physics.SphereCast(origin, sphereRadius, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            currentHitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;
        }
        else
        {
            currentHitDistance = maxDistance;
            currentHitObject = null;
        }

        if(currentHitObject.tag == "Debris" && !slowed)
        {
            slowed = true;
            controller.Acceleration = ccAccelerationDuringSlow;
            Debug.Log("Slowed");
        }
        else if(slowed && timer >= slowDuration)
        {
            timer = 0f;
            slowed = false;
            controller.Acceleration = ccAccelerationDefault;
            Debug.Log("Un-Slowed");
        }
        else if(currentHitObject == null)
        {
            currentHitObject = this.gameObject;
        }

        timer += Time.deltaTime;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }
}
