using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    public void OnClickStartButton()
    {
         SceneManager.LoadScene(1);
    }
    public void GameExit()
    {
        Application.Quit();
    }


}
