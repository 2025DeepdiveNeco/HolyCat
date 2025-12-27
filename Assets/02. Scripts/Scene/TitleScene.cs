using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    public void OnClickStartButton()
    {
        SceneManager.LoadScene(2);
    }
    public void GameExit()
    {
        Application.Quit();
    }


}
