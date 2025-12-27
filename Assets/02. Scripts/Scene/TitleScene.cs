using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    public void OnClickStartButton()
    {
        StartCoroutine(LoadGameCo());
    }
    public void GameExit()
    {
        Application.Quit();
    }

    IEnumerator LoadGameCo()
    {
        var _asyncOperation = SceneManager.LoadSceneAsync(2);
        _asyncOperation.allowSceneActivation = false;

        if (_asyncOperation == null)
        {
            yield break;
        }

        yield return new WaitForSeconds(2f);



        while (!_asyncOperation.isDone)
        {
            _asyncOperation.allowSceneActivation = true;
            yield break;
        }
    }
}
