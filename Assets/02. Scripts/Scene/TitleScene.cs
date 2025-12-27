using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    public void OnClickStartButton()
    {
        SceneLoader.Instance.LoadWithDelay("Ik", 1.5f);
    }
    public void GameExit()
    {
        Application.Quit();
    }
}