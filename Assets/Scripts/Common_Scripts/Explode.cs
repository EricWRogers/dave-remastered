using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Explode : MonoBehaviour
{
    [HideInInspector] public bool onGameObject = false;
    [HideInInspector] public float radius = 10f;
    [HideInInspector] public float damage = 10f;
    [HideInInspector] public float force = 5f;
    [HideInInspector] public float lift = 5f;

    public void DoExplosion(float radius, float damage, float force, float lift)
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);    //checks for colliders in a radius around the explosion position
        foreach (Collider hit in colliders)     //for every collider in the array
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();       //gets the rigidbody for this collider

            if (hit.tag == "Player")        //if the this collider is the player...
            {
                hit.GetComponent<Health>().TakeDamage(damage);
            }

            if (rb != null)    //if it has a rigidbody and isn't a soundwave
                rb.AddExplosionForce(force, explosionPos, radius, lift, ForceMode.Impulse);    //adds explosion force in all directions

            Destroy(gameObject);    //destroys the actual case itself
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (onGameObject)
        {
            DoExplosion(radius, damage, force, lift);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Explode))]
public class ExplodeScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        Explode script = (Explode)target;

        // draw checkbox for the bool
        script.onGameObject = EditorGUILayout.Toggle("On Game Object?", script.onGameObject);
        if (script.onGameObject) // if bool is true, show other fields
        {
            script.radius = EditorGUILayout.FloatField("Range", script.radius);
            script.damage = EditorGUILayout.FloatField("Damage", script.damage);
            script.force = EditorGUILayout.FloatField("Force", script.force);
            script.lift = EditorGUILayout.FloatField("Lift", script.lift);
        }
    }
}
#endif