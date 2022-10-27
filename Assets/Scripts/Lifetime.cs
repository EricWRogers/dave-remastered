using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    public float particleLifetime = 1.5f;

    public GameObject thisObject;
    // Update is called once per frame
    void Update()
    {
        Destroy(thisObject, particleLifetime);
    }
}
