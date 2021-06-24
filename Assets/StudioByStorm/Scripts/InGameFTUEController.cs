using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

namespace StudioByStorm.Scripts
{
    public class InGameFTUEController : MonoBehaviour
    {   
        public GameObject FadePanel;
        public GameObject KnockOutPanel;

        //Sun animation stuff
        protected float sunAnimationTime = 3.0f;
        protected bool bSunAnimationPlayed = false;
        protected bool bSolarFlareTriggered = false;
        protected bool bCameraFirstTargetReached = false;
        protected bool bCameraSecondTargetReached = false;

        public GameObject Sun;
        public GameObject Camera;
        protected Quaternion CameraInitialRotation;
        public Animator SunAnimator;

        //water stuff
        public GameObject water;
        public GameObject waterStartPosition;
        protected Vector3 waterEndPosition;
        
        public GameObject iceberg;//the iceberg
        public GameObject icebergStartPosition;//the ftue start position
        protected Vector3 icebergEndPosition;//the position to lerp to
        protected float iceBergMovementThreshold = 0.5f;//a threshold to help with lerping

        public CameraController cameraController;
        public PlayerController playerController;

        //intro text animation
        protected float introTextAnimationDelay = 2.0f;//the initial delay after scene loads 
        public Text introText;
        protected char[] introTextChars;
        protected bool bIntroTextAnimationPlayed = false;
        protected bool bIntroTextAnimationPlaying = false;

        //iceberg animation
        protected float iceBergAnimationDelayTime = 0.5f;//the intial delay after text animation
        protected bool iceBergAnimationPlayed = false;
        protected bool bCameraShook = false;

        //player knockout animation
        protected float playerKnockoutAnimationDelayTime = 1.0f;//the delay until player knockout.. should be after icebergdelay
        protected bool bPlayerKnockoutAnimationPlayed = false;

        //player wakeup animation
        protected float playerWakeUpAnimationDelayTime = 3.5f;
        protected bool bPlayerWakeUpAnimationPlayed = false;

        //text animation
        public GameObject dialog;
        public AudioSource typeWriterSound;
        protected float textAnimationDelayTime = 2.0f;
        protected bool bTextAnimationPlayed = false;
        protected bool bTextAnimationPlaying = false;
        public Text currentText;
        protected string currentTextString;
        public Text firstText;
        public Text secondText;
        public Text thirdText;
        protected char[] firstTextChars;
        protected char[] secondTextChars;
        protected char[] thirdTextChars;

        public void Start()
        {
            FadePanel.SetActive(true);

            CameraInitialRotation = Camera.transform.rotation;

            waterEndPosition = water.transform.position;
            icebergEndPosition = iceberg.transform.position;
            icebergEndPosition.y = 0.0f;


            water.transform.position = waterStartPosition.transform.position;
            iceberg.transform.position = icebergStartPosition.transform.position;

            introTextChars = introText.text.ToCharArray();
            firstTextChars = firstText.text.ToCharArray();
            secondTextChars = secondText.text.ToCharArray();
            thirdTextChars = thirdText.text.ToCharArray();
        }

        public void Update()
        {
            //intro text animation
            if (! bIntroTextAnimationPlayed || bIntroTextAnimationPlaying) {
                if (! bIntroTextAnimationPlaying) {
                    if (introTextAnimationDelay > 0.0f) {
                        introTextAnimationDelay -= Time.deltaTime;
                    } else {
                        bIntroTextAnimationPlaying = true;
                        StartCoroutine(introTextAnimation());
                    }
                }
                return;
            }

            //Sun animation
            if (!bSunAnimationPlayed) {
                playSunAnimation();
                return;
            }

            //Second camera target animation
            if (!bCameraSecondTargetReached) {
                if (Camera.transform.rotation != CameraInitialRotation) {
                    Camera.transform.rotation = Quaternion.Lerp(Camera.transform.rotation, CameraInitialRotation, Time.deltaTime * 3.0f);
                } else {
                    bCameraSecondTargetReached = true;
                    Vector3 LevelContainerTargetPos = GameController.Instance.LevelController.LevelContainer.transform.position;
                    LevelContainerTargetPos.z = 0.0f;
                    GameController.Instance.LevelController.LevelContainer.transform.position = LevelContainerTargetPos;
                }   
            }

            //iceberg animation
            if (!iceBergAnimationPlayed) {
                playIceBergAnimation();
            }

            //player knockout animation
            if (!bPlayerKnockoutAnimationPlayed) {
                PlayerKnockoutAnimation();
                return;
            }

            //player wakeup animation
            if (!bPlayerWakeUpAnimationPlayed) {
                PlayerWakeUpAnimation();
                return;
            }

            //text animation
            if (! bTextAnimationPlayed || bTextAnimationPlaying) {
                if (! bTextAnimationPlaying) {
                    if (textAnimationDelayTime > 0.0f) {
                        textAnimationDelayTime -= Time.deltaTime;
                    } else {
                        FadePanel.SetActive(false);
                        bTextAnimationPlaying = true;
                        StartCoroutine(textAnimation());
                    }
                }
                return;
            }

            FTUEOver();
        }

        protected void playSunAnimation()
        {
            if (!bCameraFirstTargetReached) {
                var modifiedSunPosition = Sun.transform.position;
                modifiedSunPosition.y = Sun.transform.position.y - 100.0f;
                var lookPos = modifiedSunPosition - Camera.transform.position;
                var targetRotation = Quaternion.LookRotation(lookPos);
      
                if (Camera.transform.rotation != targetRotation) {
                    Camera.transform.rotation = Quaternion.Slerp(Camera.transform.rotation, targetRotation, Time.deltaTime * 2.0f);
                } else {
                    bCameraFirstTargetReached = true;
                }   

                return;
            }

            if (!bSolarFlareTriggered) {
                bSolarFlareTriggered = true;
                SunAnimator.SetTrigger("flare");
            }

            if (sunAnimationTime > 0.0f) {
                sunAnimationTime -= Time.deltaTime;
                return;
            }

            bSunAnimationPlayed = true;
        }

        protected void playIceBergAnimation()
        {
            if (iceBergAnimationDelayTime > 0.0f) {
                iceBergAnimationDelayTime -= Time.deltaTime;
                return;
            }

            if (! bCameraShook) {
                bCameraShook = true;
                cameraController.Shake();
                AudioController.Singleton.PlayGrumblingSound();
            }

            if (bCameraShook && (iceberg.transform.position.y - iceBergMovementThreshold >= icebergEndPosition.y)) {
                iceberg.transform.position = Vector3.Lerp(iceberg.transform.position, icebergEndPosition, 0.025f);
                water.transform.position = Vector3.Lerp(water.transform.position, waterEndPosition, 0.05f);
            } else {
                iceBergAnimationPlayed = true;
            }
        }

        protected void PlayerKnockoutAnimation()
        {
            if (playerKnockoutAnimationDelayTime > 0.0f) {
                playerKnockoutAnimationDelayTime -= Time.deltaTime;
                return;
            } else {
                bPlayerKnockoutAnimationPlayed = true;
                GameController.Instance.FTUEStart();
                playerController.KnockOut();
                KnockOutPanel.SetActive(true);
            }
        }

        protected void PlayerWakeUpAnimation()
        {
            if (playerWakeUpAnimationDelayTime > 0.0f) {
                playerWakeUpAnimationDelayTime -= Time.deltaTime;
                return;
            } else {
                bPlayerWakeUpAnimationPlayed = true;
                playerController.WakeUp();
                KnockOutPanel.SetActive(false);
            }
        }

        protected IEnumerator introTextAnimation()
        {
            dialog.SetActive(true);
            currentText.text = "";
            currentTextString = "";
            currentText.gameObject.SetActive(true);

            foreach (char letter in introTextChars)
            {
                typeWriterSound.Play();
                currentTextString += letter;
                currentText.text = currentTextString;      
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(2.0f);

            dialog.SetActive(false);
            bIntroTextAnimationPlayed = true;
            bIntroTextAnimationPlaying = false;
        }

        protected IEnumerator textAnimation()
        {
            dialog.SetActive(true);
            currentText.text = "";
            currentTextString = "";
            currentText.gameObject.SetActive(true);

            foreach (char letter1 in firstTextChars)
            {
                typeWriterSound.Play();
                currentTextString += letter1;
                currentText.text = currentTextString;      
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(3.0f);

            currentText.text = "";
            currentTextString = "";
            foreach (char letter2 in secondTextChars)
            {
                typeWriterSound.Play();
                currentTextString += letter2;
                currentText.text = currentTextString;      
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(3.0f);

            currentText.text = "";
            currentTextString = "";
            foreach (char letter3 in thirdTextChars)
            {
                typeWriterSound.Play();
                currentTextString += letter3;
                currentText.text = currentTextString;      
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(3.0f);

            bTextAnimationPlayed = true;
            bTextAnimationPlaying = false;
        }

        protected void FTUEOver()
        {
            Sun.SetActive(false);
            GameController.Instance.PlayGameButtonClick();
            ZPlayerPrefs.SetInt("InGameFTUE", 1);
            gameObject.SetActive(false);
        }
    }
}