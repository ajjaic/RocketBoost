using UnityEngine;

// TODO: this class should be scriptable object
public class SessionManager : MonoBehaviour
{
    [SerializeField] private RocketInputHandler player; // TODO: cannot have direct reference to the player. remove and
                                                        // replace with scriptable object
    [SerializeField] private GameEvent playerDeathEvent = null;
    [SerializeField] private GameEvent playerAtLvlEndEvent = null;
    [SerializeField] private float sceneLoadDelay = 0f;
    
    // messages
    private void Update()
    {
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                StartCoroutine(SceneLoader.LoadNextLevel(0));
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                player.ToggleCollision();
            }
        }
    }

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

