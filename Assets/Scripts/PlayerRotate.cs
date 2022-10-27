using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] float turnSpeed = 20f;

    void Update()
    {
        Vector2 stickPos = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        float horizontalInput = stickPos.x;
        transform.Rotate(Vector3.up * horizontalInput * turnSpeed * Time.deltaTime);
    }
}
