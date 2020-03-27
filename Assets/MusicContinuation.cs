using UnityEngine;

public class MusicContinuation : MonoBehaviour
{
    private AudioSource _audioSource;

    private int _pcmIndex;
    private const string SEEKINDEX = "SeekIndex";
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        
        if (PlayerPrefs.HasKey(SEEKINDEX))
        {
            _audioSource.timeSamples = PlayerPrefs.GetInt(SEEKINDEX);
        }
        else
        {
            _audioSource.timeSamples = 0;
        }
    }
    
    private void Update()
    {
        _pcmIndex = _audioSource.timeSamples;
    }
    
    private void OnDisable()
    {
        PlayerPrefs.SetInt(SEEKINDEX, _pcmIndex);
    }
}
