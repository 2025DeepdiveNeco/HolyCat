using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonBehaviour<SceneLoader>
{
    public void LoadWithDelay(string sceneName, float delay)
    {
        StartCoroutine(LoadSceneDelayRoutine(sceneName, delay));
    }

    System.Collections.IEnumerator LoadSceneDelayRoutine(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}