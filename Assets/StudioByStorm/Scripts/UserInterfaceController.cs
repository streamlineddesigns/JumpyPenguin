using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserInterfaceController : MonoBehaviour
{
    public Text scoreText;
    public GameObject PauseView;
    public GameObject GameView;

    public void updateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void PauseGame()
    {
        GameView.SetActive(false);
        PauseView.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        PauseView.SetActive(false);
        GameView.SetActive(true);
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
