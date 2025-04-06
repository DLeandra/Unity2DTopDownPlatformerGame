using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicManager : MonoBehaviour
{
    // Reference to the Game Over screen
    public GameObject gameOverScreen;

    // Show the Game Over screen
    public void gameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            Time.timeScale = 0f; // Optional: Pause the game
        }
        else
        {
            Debug.LogWarning("Game Over screen not assigned!");
        }
    }

    // Restart the current scene
    public void restartGame()
    {
        Time.timeScale = 1f; // Reset time scale
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Quit the application
    public void quitGame()
    {
        Debug.Log("Quit button pressed");
        Application.Quit();
    }
}
