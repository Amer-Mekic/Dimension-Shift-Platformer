using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseCanvas;

    public void Pause()
    {
        PauseCanvas.SetActive(true);
        Time.timeScale=0;
    }
    public void Resume()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale=1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

public void QuitGame()
    {
        Application.Quit();
    }
}