using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace StudioByStorm.Scripts
{
    public class UserInterfaceController : MonoBehaviour
    {
        public Text scoreText;
        public Text gameOverScoreText;
        public GameObject PauseView;
        public GameObject GameView;
        public GameObject GameOverView;
        public GameObject FrostView;
        public bool isDialogBeingClicked;

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
            StartCoroutine(DelayedGameOver());
        }

        IEnumerator DelayedGameOver()
        {
            Time.timeScale = 1;
            yield return new WaitForSeconds(3.0f);
            gameOverScoreText.text = GameController.Instance.LevelController.getCurrentScore().ToString();
            GameOverView.SetActive(true);
            Time.timeScale = 0;
        }

        public void FrostCamera()
        {
            FrostView.SetActive(true);
        }

        public void OnDialogPointerDown()
        {
            isDialogBeingClicked = true;
        }

        public void OnDialogPointerUp()
        {
            isDialogBeingClicked = false;
        }
    }
}