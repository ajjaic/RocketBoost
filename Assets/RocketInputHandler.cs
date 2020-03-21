using System;
using UnityEngine;

// TODO: stop thrust afx when player dead
public class RocketInputHandler : MonoBehaviour
{
    #pragma warning disable 649
    [Header("Event")]
    [SerializeField] private GameEvent playerDeathEvent;
    [SerializeField] private GameEvent playerAtLvlEndEvent;
    [Header("Movement")]
    [SerializeField] private float forwardVelocity; 
    [SerializeField] private float rotationVelocity;
    [Header("Audio")]
    [SerializeField] private AudioClip mainThruster;
    [SerializeField] private AudioClip deathExplosion;
    [SerializeField] private AudioClip levelFinished;
    #pragma warning restore 649
    
    private Rigidbody _rigidBody;
    private AudioSource _audioSource; 
    
    private Vector3 _forceVector; // thrust
    private Vector3 _rotationVector; // rotations
    private State _state = State.Alive; // current player state

    // messages
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // TODO: replace with state switch statement
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
        if (_state == State.Dead || _state == State.LevelComplete) { return; }
        
        if (!other.gameObject.CompareTag("Friendly") && !other.gameObject.CompareTag("Finish"))
        {
            _state = State.Dead;
            enabled = false;
            HandleAudio();
            playerDeathEvent.RaiseEvent();
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            _state = State.LevelComplete;
            enabled = false;
            HandleAudio();
            playerAtLvlEndEvent.RaiseEvent();
        }
    }

    // methods
    private void HandleThrust()
    {
        if (Input.GetButton("Thrust"))
        {
            _state = State.AliveAndThrusting;
            _forceVector = Vector3.up * forwardVelocity;
        }
        else
        {
            _state = State.AliveAndNotThrusting;
            _forceVector = Vector3.zero;
        }
        HandleAudio();
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
    
    private void HandleAudio()
    {
        switch (_state)
        {
            case State.AliveAndThrusting:
                if (!_audioSource.isPlaying)
                    _audioSource.PlayOneShot(mainThruster);
                break;
            
            case State.AliveAndNotThrusting:
                _audioSource.Stop();
                break;
            
            case State.Dead:
                _audioSource.Stop();
                _audioSource.PlayOneShot(deathExplosion);
                break;
            
            case State.LevelComplete:
                _audioSource.Stop();
                _audioSource.PlayOneShot(levelFinished);
                break;
        }
    }

    enum State
    {
        Alive,
        Dead,
        AliveAndThrusting,
        AliveAndNotThrusting,
        LevelComplete
    }
}
