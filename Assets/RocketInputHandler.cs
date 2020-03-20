using System;
using UnityEngine;

public class RocketInputHandler : MonoBehaviour
{
    private Rigidbody _rigidBody;
    [SerializeField] private float forwardVelocity; // thrust
    private Vector3 _forceVector;
    [SerializeField] private float rotationVelocity; // rotations
    private Vector3 _rotationVector;
    private AudioSource _audioSource; // audio

    // messages
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleThrust();
        HandleRotation();
    }

    void FixedUpdate()
    {
        _rigidBody.AddRelativeForce(_forceVector);
        if (Math.Abs(_rotationVector.z) > Mathf.Epsilon)
        {
            // Bug: After freezerotation is over, x and y axis rotation is not constrained again.
            _rigidBody.freezeRotation = true;
            var rotationDelta = Quaternion.Euler(_rotationVector);
            _rigidBody.MoveRotation((_rigidBody.rotation * rotationDelta));
            _rigidBody.freezeRotation = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Friendly"))
        {
            print("DIE"); 
        } 
    }

    // methods
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
