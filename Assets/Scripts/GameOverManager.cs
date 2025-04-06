using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{

    public GameObject gameOverScreen; // Assign this in the Inspector

    void Start()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    public void restartGame()
    {
        // Reset the score to 0
        Scoring.totalScore = 0;


        // Reset the time scale in case it was paused
        Time.timeScale = 1f;

        // Reset health
        Health.totalHealth = 1f;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Disable Game Over screen on restart
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }


    public void quitGame()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }
}
