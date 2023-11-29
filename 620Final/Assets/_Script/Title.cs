using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public GameObject credits;
    public GameObject cancelCredits;
    public GameObject openCredits;
    public GameObject options;
    public GameObject controls; 
    public void StartGame()
    {
        GameManager.instance.LoadNewGame();
    }

    public void LoadGame()
    {
        options.SetActive(false);
        controls.SetActive(true);
        cancelCredits.SetActive(true);
    }

    public void Credits()
    {
        options.SetActive(false);
        credits.SetActive(true);
        cancelCredits.SetActive(true);
    }

    public void CloseCredits()
    {
        credits.SetActive(false);
        controls.SetActive(false);
        cancelCredits.SetActive(false);
        options.SetActive(true);
    }

    public void Exitgame()
    {
        Application.Quit();
    }
}
