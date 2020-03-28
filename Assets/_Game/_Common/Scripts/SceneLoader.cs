using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader 
{
    public static IEnumerator LoadFirstLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(0);
    }

    public static IEnumerator LoadCurrentLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public static IEnumerator LoadNextLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        var newSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (newSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(newSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
