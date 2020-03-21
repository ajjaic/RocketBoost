using System;
using UnityEngine;

// TODO: stop thrust afx when player dead
public class RocketInputHandler : MonoBehaviour
{
    [SerializeField] private GameEvent playerDeathEvent = null;
    [SerializeField] private GameEvent playerAtLvlEndEvent = null;
    private Rigidbody _rigidBody;
    [SerializeField] private float forwardVelocity = 0f; // thrust
    private Vector3 _forceVector;
    [SerializeField] private float rotationVelocity = 0f; // rotations
    private Vector3 _rotationVector;
    private AudioSource _audioSource; // audio
    private State _state = State.Alive; // player state

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
        
        if (_state == State.Dead)
        {
            PostDeathCleanUp();
            enabled = false;
        }
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
        if (_state != State.Alive) { return; }
        
        if (!other.gameObject.CompareTag("Friendly") && !other.gameObject.CompareTag("Finish"))
        {
            _state = State.Dead;
            playerDeathEvent.RaiseEvent();
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            _state = State.Dead;
            playerAtLvlEndEvent.RaiseEvent();
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

    private void PostDeathCleanUp()
    {
        _audioSource.Stop();
    }
    
    enum State
    {
        Alive,
        Dead
    }
}
