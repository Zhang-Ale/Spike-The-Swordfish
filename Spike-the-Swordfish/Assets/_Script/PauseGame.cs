using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public bool gameIsPaused;
    public GameObject pauseMenu;
    bool noPause;
    public PlayerMovement PM;
    public GameObject deathMenu; 

    void FixedUpdate()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            Pause();
        }

        if (PM.dead)
        {
            deathMenu.SetActive(true); 
        }
    }

    void Pause()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseMenu.SetActive(true);
            //etc
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseMenu.SetActive(false);
            //etc
        }
    }

    public void ResumeGame()
    {
        gameIsPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
