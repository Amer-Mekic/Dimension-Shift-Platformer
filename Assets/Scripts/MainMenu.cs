using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level_Tutorial 1");
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("SelectLevel");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}