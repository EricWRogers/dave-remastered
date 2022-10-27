using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearBehavior : MonoBehaviour
{
    public float lifespanSeconds = 1.0f;
    private float timer = 0.0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= lifespanSeconds)
        {
            Destroy(gameObject);
        }
    }
}