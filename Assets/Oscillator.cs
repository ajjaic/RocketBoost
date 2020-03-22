using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, 8f);
    private Rigidbody _rigidbody;
    private Vector3 _startingPos;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startingPos = _rigidbody.position;
    }

    private void FixedUpdate()
    {
        var cycles = Mathf.Sin(Time.time);
        var offsetPos = offset * cycles;
        _rigidbody.position = _startingPos + offsetPos;
    }
}
