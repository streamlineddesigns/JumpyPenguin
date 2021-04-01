using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;
        public GameObject Player;
        public Camera Camera;
        protected float playerWidth = 1.3f;
        protected float RightBounds;
        protected float LeftBounds;
        protected Vector3 targetRightPos;
        protected Vector3 targetLeftPos;
        public GameObject SplashView;
        public GameObject FTUE;
        public GameObject StartView;
        public GameObject GameView;
        public Animator PlatformRiseAnimator;
        public Animator[] LevelDropAnimator;
        private bool _isGameActive;
        public bool isGameActive { 
            get{ return _isGameActive; }
        }
        private bool _isGameOver;
        public bool isGameOver { 
            get{ return _isGameOver; }
        }
        public bool canPlayerMove;

        public LevelController LevelController;
        public UserInterfaceController UserInterfaceController;
        public enum DeathType {
            Enemy,
            Water,
            Ice,
        };
        public bool bPlayerJumpOverride = false;

        void Awake()
        {
            if (GameController.Instance == null) {
                GameController.Instance = this;
                RightBounds = Screen.width;
                LeftBounds = 0;
                canPlayerMove = true;
            }
        }

        void Start()
        {
            if (ZPlayerPrefs.GetInt("InGameFTUE") == 1) {
                SplashView.SetActive(true);
            } else {
                FTUE.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (outRightBounds()) {

                canPlayerMove = false;
                Vector3 targetLeftPos = new Vector3(- Player.transform.position.x - 2 - playerWidth, Player.transform.position.y, Player.transform.position.z);
                Player.transform.position = targetLeftPos;

            } else if (outLeftBounds()) {

                canPlayerMove = false;
                Vector3 targetRightPos = new Vector3(Mathf.Abs(Player.transform.position.x) - 3.4f - playerWidth, Player.transform.position.y, Player.transform.position.z);
                Player.transform.position = targetRightPos;

            } else {

                canPlayerMove = true;

            }
        }

        public void PlayGameButtonClick() {
            StartCoroutine(PlayGame());
        }

        protected IEnumerator PlayGame() {
            yield return new WaitForSeconds(0.5f);
            StartView.SetActive(false);
            PlatformRiseAnimator.SetTrigger("PopUp");
            for(int i = 0; i < LevelDropAnimator.Length; i++) {
                LevelDropAnimator[i].SetTrigger("Drop");
            }
            yield return new WaitForSeconds(1.5f);
            GameView.SetActive(true);
            _isGameActive = true;
            LevelController.spawnNewLevel();
        }

        public void FTUEStart()
        {
            _isGameActive = true;
        }

        protected bool outRightBounds()
        {
            targetRightPos = new Vector3(Player.transform.position.x - playerWidth + 0.4f, Player.transform.position.y, Player.transform.position.z);
            return (Camera.WorldToScreenPoint(targetRightPos).x > RightBounds);
        }

        protected bool outLeftBounds()
        {
            targetLeftPos = new Vector3(Player.transform.position.x + playerWidth - 0.4f, Player.transform.position.y, Player.transform.position.z);
            return (Camera.WorldToScreenPoint(targetLeftPos).x < LeftBounds);
        }

        public void GameOver(DeathType howPlayerDied)
        {
            if (howPlayerDied == DeathType.Water) {
                AudioController.Singleton.PlayWaterSplashSound();
            } else if (howPlayerDied == DeathType.Enemy) {
                AudioController.Singleton.PlayWolfAttackSound();
            }
            _isGameActive = false;
            _isGameOver = true;
            UserInterfaceController.GameOver();
        }
    }
}