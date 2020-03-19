using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RocketInputHandler : MonoBehaviour
{
    private Rigidbody _rigidBody;
    
    // Thrust
    [SerializeField] private float forwardVelocity;
    private Vector3 _forceVector;
    
    // Rotations
    [SerializeField] private float rotationVelocity;
    private Vector3 _rotationVector;
    
    // Audio
    private AudioSource _audioSource;

    // messages
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessInput();
    }


    void FixedUpdate()
    {
        _rigidBody.AddRelativeForce(_forceVector);
        if (Math.Abs(_rotationVector.z) > Mathf.Epsilon)
        {
            var rotationDelta = Quaternion.Euler(_rotationVector);
            _rigidBody.MoveRotation((_rigidBody.rotation * rotationDelta));
        }
    }
    
    private void ProcessInput()
    {
        HandleThrust();
        HandleRotation();
    }

    private void HandleThrust()
    {
        if (Input.GetButton("Thrust"))
        {
            if (!_audioSource.isPlaying)
                _audioSource.Play();
            _forceVector = Vector3.up * forwardVelocity;
        }
        else
        {
            _audioSource.Stop();
            _forceVector = Vector3.zero;
        }
    }

    private void HandleRotation()
    {
        if (Input.GetButton("RotateLeft"))
        {
            _rotationVector.z = rotationVelocity;
        }
        else if (Input.GetButton("RotateRight"))
        {
            _rotationVector.z = -rotationVelocity;
        }
        else
        {
            _rotationVector.z = 0;
        }
    }
}
