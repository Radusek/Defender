using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadHighScores : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public GameObject mainMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        //PlayerPrefs.DeleteAll();

        /* Example data
        for (int i = 0; i < 8; i++)
        {
            string name = "oskahab";
            int value = 500000 - 2 * i - i * i / 3;

            PlayerPrefs.SetString("score" + i.ToString(), ((i+1).ToString()+".").PadRight(3) + name.PadRight(12) + value.ToString().PadRight(9) + (101-i).ToString().PadRight(4));
            PlayerPrefs.SetInt("scoreValue" + i.ToString(), value);
        }
        */

        /* Empty table
        for (int i = 0; i < 8; i++)
        {
            string name = "-";

            PlayerPrefs.SetString("score" + i.ToString(), name.PadRight(12) + 0.ToString().PadRight(9) + (0).ToString().PadRight(4));
            PlayerPrefs.SetInt("scoreValue" + i.ToString(), 0);
        }
        */

        scoreText.text = "   " + "PLAYER".PadRight(12) + "SCORE".PadRight(9) + "WAVE".PadRight(4) + "\n\n";

        for (int i = 0; i < 8; i++)
        {
            string keyString = "score" + i.ToString();
            
            scoreText.text += (i+1).ToString()+". " + PlayerPrefs.GetString(keyString) + "\n";
        }
    }
}
