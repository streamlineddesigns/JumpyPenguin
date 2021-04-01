using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudioByStorm.Scripts
{
    public class InGameFTUEController : MonoBehaviour
    {   
        public GameObject water;
        public GameObject waterStartPosition;
        protected Vector3 waterEndPosition;
        
        public GameObject iceberg;//the iceberg
        public GameObject icebergStartPosition;//the ftue start position
        protected Vector3 icebergEndPosition;//the position to lerp to
        protected float iceBergMovementThreshold = 0.5f;//a threshold to help with lerping

        public CameraController cameraController;
        public PlayerController playerController;

        //iceberg animation
        protected float iceBergAnimationDelayTime = 0.5f;
        protected bool iceBergAnimationPlayed = false;
        protected bool bCameraShook = false;

        //player knockout animation
        protected float playerKnockoutAnimationDelayTime = 1.0f;
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
            waterEndPosition = water.transform.position;
            icebergEndPosition = iceberg.transform.position;
            icebergEndPosition.y = 0.0f;


            water.transform.position = waterStartPosition.transform.position;
            iceberg.transform.position = icebergStartPosition.transform.position;


            firstTextChars = firstText.text.ToCharArray();
            secondTextChars = secondText.text.ToCharArray();
            thirdTextChars = thirdText.text.ToCharArray();
        }

        public void Update()
        {
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
                        bTextAnimationPlaying = true;
                        StartCoroutine(textAnimation());
                    }
                }
                return;
            }

            FTUEOver();
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
            }
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
            gameObject.SetActive(false);
            GameController.Instance.PlayGameButtonClick();
        }
    }
}