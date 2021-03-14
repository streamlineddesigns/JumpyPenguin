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
    public GameObject GameOverView;
    public GameObject FrostView;

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

    public void GameOver()
    {
        GameView.SetActive(false);
        PauseView.SetActive(false);
        Time.timeScale = 1;
        StartCoroutine(DelayedGameOver());
    }

    IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(3.0f);
        GameOverView.SetActive(true);
    }

    public void FrostCamera()
    {
        FrostView.SetActive(true);
    }
}
