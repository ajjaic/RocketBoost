using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RocketInputHandler : MonoBehaviour
{
    [SerializeField] private float forwardVelocity;
    private Rigidbody _rigidBody;
    private Vector3 _forceVector;

    // messages
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ProcessInput();
    }

    void FixedUpdate()
    {
        _rigidBody.AddRelativeForce(_forceVector);
    }
    
    private void ProcessInput()
    {
        if (Input.GetButton("Thrust"))
            _forceVector = Vector3.up * forwardVelocity;
        else
            _forceVector = Vector3.zero;

        if (Input.GetButton("RotateLeft"))
        {
            print("rotating left");   
        }
        else if (Input.GetButton("RotateRight"))
        {
            print("rotating right");   
        }
    }
}
