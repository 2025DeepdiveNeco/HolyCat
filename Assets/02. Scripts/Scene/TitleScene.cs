using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    public void OnClickStartButton()
    {
        int index = PlayerPrefs.HasKey("Stage") ? PlayerPrefs.GetInt("Stage") : 1;
        SceneLoader.Instance.LoadWithDelay($"Stage{index}", 1.5f);
    }
    public void GameExit()
    {
        Application.Quit();
    }
}