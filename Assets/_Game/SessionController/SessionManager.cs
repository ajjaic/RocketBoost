using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: this class should be scriptable object
public class SessionManager : MonoBehaviour
{
    [SerializeField] private GameEvent playerDeathEvent = null;
    [SerializeField] private GameEvent playerAtLvlEndEvent = null;
    [SerializeField] private float sceneLoadDelay = 0f;
    
    // messages
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

