using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class testScript : MonoBehaviour
{
public void MainMenuSwitch()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOverSwitch()
    {
        SceneManager.LoadScene("GameOver");
    }

}