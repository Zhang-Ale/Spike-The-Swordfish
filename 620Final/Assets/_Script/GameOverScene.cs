using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReturnStartMenu()
    {
        SceneManager.LoadScene("ManagerScene");
        Time.timeScale = 1;
    }

    public void Exitgame()
    {
        Application.Quit();
    }
}
