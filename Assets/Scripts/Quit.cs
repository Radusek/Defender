using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{
    public void CloseTheGame()
    {
        GameManager.Instance.quittingScene = true;
        SceneManager.LoadScene(0);
    }
}
