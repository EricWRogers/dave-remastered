using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] private float threshold = .1f; //Precentage of button pressed needed to activate button
    [SerializeField] private float deadZone = 0.050f; //Ensures there is no movement within a certain range that accidently activates the button.

    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;

    public UnityEvent onPressed, onReleased;
   
    void Start()
    {
        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
    }

    
    void Update()
    {
        if (!_isPressed && GetValue() + threshold >= 1)
            Pressed();
        if (_isPressed && GetValue() - threshold <= 0)
            Released();

    }

    private float GetValue()
    {
        var value = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;

        if (Math.Abs(value) < deadZone)
            value = 0;

        return Mathf.Clamp(value, -1f, 1f);
    }



    private void Pressed()
    {
        _isPressed = true;
        onPressed.Invoke();
       // GetComponent<Renderer>().material.color = Color.green; //Changes color to green
        Debug.Log("Pressed");
    }

    private void Released()
    {
        _isPressed = false;
        onReleased.Invoke();
        //GetComponent<Renderer>().material.color = Color.red; //Changes color back to red
        Debug.Log("Released");
    }
}
