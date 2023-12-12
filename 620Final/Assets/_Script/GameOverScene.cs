using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour
{
    public GameObject story1, story2, continuebutton, backButton; 
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

    public void OpenStory2()
    {
        story1.SetActive(false);
        story2.SetActive(true);
        continuebutton.SetActive(false);
        backButton.SetActive(true); 
    }

    public void OpenStory1()
    {
        backButton.SetActive(false);
        continuebutton.SetActive(true);
        story2.SetActive(false);
        story1.SetActive(true);
    }
}
