using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEvents : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject highScores;


    public void StartPlaying()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowHighScores()
    {
        mainMenu.SetActive(false);
        highScores.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
