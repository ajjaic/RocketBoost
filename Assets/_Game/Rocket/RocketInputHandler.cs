using System;
using UnityEngine;

public class RocketInputHandler : MonoBehaviour
{
    #pragma warning disable 649
    [Header("ScriptableVariables")] 
    [SerializeField] private BoolVariable varColliderDisabler;
    [Header("Event")]
    [SerializeField] private GameEvent playerDeathEvent;
    [SerializeField] private GameEvent playerAtLvlEndEvent;
    [Header("Movement")]
    [SerializeField] private float forwardVelocity; 
    [SerializeField] private float rotationVelocity;
    [Header("Audio")]
    [SerializeField] private AudioClip mainThrusterAudio;
    [SerializeField] private AudioClip deathExplosionAudio;
    [SerializeField] private AudioClip levelFinishedAudio;
    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem deathExplosionParticleSys;
    [SerializeField] private ParticleSystem levelFinishedParticleSys;
    #pragma warning restore 649
    
    private Rigidbody _rigidBody;
    private AudioSource _audioSource;

    private ParticleSystem _mainThrusterParticleSys;
    private Vector3 _forceVector, _rotationVector; // thrust and rotations
    private State _state = State.Alive; // current player state
    
    enum State
    {
        Alive,
        Dead,
        Thrusting,
        LevelComplete
    }

    // messages
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _mainThrusterParticleSys = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        HandleThrust();
        HandleRotation();
    }
    
    void FixedUpdate()
    {
        _rigidBody.AddRelativeForce(_forceVector);
        if (Math.Abs(_rotationVector.z) > Mathf.Epsilon)
        {
            _rigidBody.angularVelocity = Vector3.zero;
            var rotationDelta = Quaternion.Euler(_rotationVector);
            _rigidBody.MoveRotation((_rigidBody.rotation * rotationDelta));
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (_state == State.Dead || _state == State.LevelComplete || varColliderDisabler.GetBool()) { return; }
        
        if (!other.gameObject.CompareTag("Friendly") && !other.gameObject.CompareTag("Finish"))
        {
            _state = State.Dead;
            HandleAudio();
            HandleParticles();
            HandleDeath();
            // playerDeathEvent.RaiseEvent();
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            _state = State.LevelComplete;
            HandleAudio();
            HandleParticles();
            HandleLevelComplete();
            playerAtLvlEndEvent.RaiseEvent();
        }
    }

    // methods
    private void HandleLevelComplete()
    {
        if (_state == State.LevelComplete)
        {
            enabled = false;
            Instantiate(levelFinishedParticleSys);
        }
    }

    private void HandleDeath()
    {
        if (_state == State.Dead)
        {
            enabled = false;
            Instantiate(deathExplosionParticleSys, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void HandleThrust()
    {
        if (Input.GetButton("Thrust"))
        {
            _state = State.Thrusting;
            _forceVector = Vector3.up * forwardVelocity;
        }
        else
        {
            _state = State.Alive;
            _forceVector = Vector3.zero;
        }
        HandleAudio();
        HandleParticles();
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

    private void HandleParticles()
    {
        switch (_state)
        {
            case State.Alive:
                _mainThrusterParticleSys.Stop(false, ParticleSystemStopBehavior.StopEmitting);
                break;
            case State.Thrusting:
                if (!_mainThrusterParticleSys.isEmitting) _mainThrusterParticleSys.Play();
                break;
            case State.Dead:
                _mainThrusterParticleSys.Stop();
                levelFinishedParticleSys.Stop();
                deathExplosionParticleSys.Play();
                break;
            case State.LevelComplete:
                _mainThrusterParticleSys.Stop();
                deathExplosionParticleSys.Stop();
                levelFinishedParticleSys.Play();
                break;
        }
    }

    private void  HandleAudio()
    {
        switch (_state)
        {
            case State.Alive:
                _audioSource.Stop();
                break;
            case State.Thrusting:
                if (!_audioSource.isPlaying) _audioSource.PlayOneShot(mainThrusterAudio);
                break;
            case State.Dead:
                _audioSource.Stop();
                AudioSource.PlayClipAtPoint(deathExplosionAudio, Camera.main.transform.position, 0.5f); // TODO add camera in editor
                break;
            case State.LevelComplete:
                _audioSource.Stop();
                _audioSource.PlayOneShot(levelFinishedAudio);
                break;
        }
    }
}
