using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levels : MonoBehaviour
{
    public void Level_1()
    {
        SceneManager.LoadScene("test1");
    }

    public void Level_2()
    {
        SceneManager.LoadScene("test1");
    }
    public void Level_3()
    {
        SceneManager.LoadScene("test1");
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}