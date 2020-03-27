using UnityEngine;

public class StarCollectible : MonoBehaviour
{
    #pragma warning disable 649
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private ParticleSystem pickedUpVfx;
    #pragma warning restore 649
    
    private Collectible _parent;
    private Rigidbody _rigidBody;

    // messages
    private void Start()
    {
        _parent = GetComponentInParent<Collectible>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Quaternion rotationDelta = Quaternion.Euler(new Vector3(0, 2, 0));
        _rigidBody.MoveRotation(_rigidBody.rotation * rotationDelta);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayPickupAfx();
        PlayPickupVfx();
        GameObject thisGameObj = gameObject;
        thisGameObj.SetActive(false); 
        Destroy(thisGameObj);
        _parent.ActivateNextStar();

    }

    // methods
    private void PlayPickupAfx()
    {
        // TODO: serialize main camera and volume as parameter in editor
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, 0.5f);
    }
    
    private void PlayPickupVfx()
    {
        Instantiate(pickedUpVfx, transform.position, transform.rotation);
    }
}
