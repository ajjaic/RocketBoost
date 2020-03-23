using UnityEngine;

public class SessionManager : MonoBehaviour
{
    #pragma warning disable 649
    [SerializeField] private GameEvent playerDeathEvent;
    [SerializeField] private GameEvent playerAtLvlEndEvent;
    [SerializeField] private float sceneLoadDelay;
    #pragma warning restore 649
    
    private void OnEnable()
    {
        playerDeathEvent.CallbackListener += PlayerHasDied;
        playerAtLvlEndEvent.CallbackListener += PlayerReachesEndOfLevel;
    }

    private void OnDisable()
    {
        playerDeathEvent.CallbackListener -= PlayerHasDied;
        playerAtLvlEndEvent.CallbackListener -= PlayerReachesEndOfLevel;
    }
    
    // methods
    private void PlayerHasDied()
    {
        StartCoroutine(SceneLoader.LoadFirstLevel(sceneLoadDelay));
    }

    private void PlayerReachesEndOfLevel()
    {
        StartCoroutine(SceneLoader.LoadNextLevel(sceneLoadDelay));
    }
}

