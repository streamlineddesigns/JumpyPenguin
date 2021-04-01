using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace StudioByStorm.Scripts.FTUE
{
    public class FTUEController : MonoBehaviour
    {
        public AudioSource typeWriterSound;
        public AudioSource timeSound;
        protected bool bAtmosphereSoundPlayed = false;
        public AudioSource atmosphereDieSound;

        public GameObject mainCamera;
        public GameObject secondaryCamera;
        public GameObject fadeOutPanel;

        protected float elapsedSyncTime = 3.0f;

        public RotateSimple earthOrbit;
        public Material EarthMaterial;
        protected float colorRStep;
        protected float colorGStep;
        protected float timeTilColor;
        protected Color currentEarthColor;
        protected Color initialEarthColor = new Color(0.02f, 0.54f, 0f);
        protected Color targetEarthColor = new Color(0.79f, 0.49f, 0f);

        public Material AtmosphereMaterial;
        protected float colorAStep;
        protected float atmosphereDelayTime = 7.0f;//delay after flare hits
        protected float timeTilAtmosphereColor = 1.0f;
        protected Color currentAtmosphereColor;
        protected Color initialAtmosphereColor = new Color(0.09f, 0.0f, 1f, 0.15f);
        protected Color targetAtmosphereColor = new Color(0.09f, 0.0f, 1f, 0.04f);

        public Text initialText;
        protected Text mainText;
        protected string currentText;
        protected char[] initialTextChars;
        public Text headerText;
        public Text yearText;

        protected bool bOutroAnimationPlayed = false;
        protected bool bOutroAnimationPlaying = false;
        public Text firstText;
        public Text secondText;
        public Text thirdText;
        protected char[] firstTextChars;
        protected char[] secondTextChars;
        protected char[] thirdTextChars;

        protected bool bTypeWriterAnimationPlayed = false;
        protected bool bTypeWriterAnimationPlaying;
        protected bool bInitialElapsedAnimationPlayed = false;
        protected float elapsedAnimationDelay = 2.0f;
        protected float flareDelayTime = 2.0f;
        protected bool belapsedAnimationDelayCompleted = false;
        protected bool bFinalElapsedAnimationPlayed = false;
        protected bool bAtmosphereAnimationPlayed = false;

        protected float speedStep;
        protected float timeTilMaxSpeed = 2.0f;
        protected float currentSpeed;
        protected float initialSpeed = 10;
        protected float targetSpeed = 1000;

        protected float yearStep;
        protected float timeTilMaxYear;
        protected float currentYear;
        protected float initialYear = 1954;
        protected float targetYear = 2154;

        protected float cameraYStep;
        protected float cameraZStep;
        protected float timeTilTargetCameraPos;
        public GameObject cameraContainer;
        protected Vector3 currentCameraPos;
        public GameObject initialCameraPos;
        public GameObject targetCameraPos;

        public Animator sunAnimator;

        void Awake()
        {
            Application.targetFrameRate = 60;
        }

        // Start is called before the first frame update
        void Start()
        {
            //time
            timeTilColor = elapsedSyncTime;
            timeTilTargetCameraPos = elapsedSyncTime;
            timeTilMaxYear = elapsedSyncTime;

            //text
            initialTextChars = initialText.text.ToCharArray();
            firstTextChars = firstText.text.ToCharArray();
            secondTextChars = secondText.text.ToCharArray();
            thirdTextChars = thirdText.text.ToCharArray();
            mainText = initialText;

            //earth mat
            currentEarthColor = initialEarthColor;
            colorRStep = (targetEarthColor.r - initialEarthColor.r) / (60.0f * timeTilColor);
            colorGStep = (targetEarthColor.g - initialEarthColor.g) / (60.0f * timeTilColor);
            setEarthColor();

            //earth atmosphere mat
            currentAtmosphereColor = initialAtmosphereColor;
            colorAStep = (targetAtmosphereColor.a - initialAtmosphereColor.a) / (60.0f * timeTilAtmosphereColor);
            setAtmosphereColor();

            //speed
            currentSpeed = initialSpeed;
            currentYear = initialYear;

            speedStep = (targetSpeed - initialSpeed) / (60.0f * timeTilMaxSpeed);
            yearStep = (targetYear - initialYear) / (60.0f * timeTilMaxYear);

            //camera
            currentCameraPos = cameraContainer.transform.position;
            cameraYStep = (targetCameraPos.transform.position.y - initialCameraPos.transform.position.y) / (60.0f * timeTilTargetCameraPos);
            cameraZStep = (targetCameraPos.transform.position.z - initialCameraPos.transform.position.z) / (60.0f * timeTilTargetCameraPos);
        }

        // Update is called once per frame
        void Update()
        {
            if (!bTypeWriterAnimationPlayed || bTypeWriterAnimationPlaying) {
                if (! bTypeWriterAnimationPlaying) {
                    bTypeWriterAnimationPlaying = true;
                    StartCoroutine(typeWriterAnimation());
                }
                return;
            }
            
            if (! bFinalElapsedAnimationPlayed) {

                simulateElapsedTime();
                return;

            } else if (!bAtmosphereAnimationPlayed) {
                fadeOutAtmosphere();
                return;

            } else if (! bOutroAnimationPlayed || bOutroAnimationPlaying) {
                if (! bOutroAnimationPlaying) {
                    bOutroAnimationPlaying = true;
                    StartCoroutine(outroTextAnimation());
                }
                return;
            } else {
                SceneManager.LoadScene("Game");
            }
        }

        protected IEnumerator typeWriterAnimation()
        {
            initialText.gameObject.SetActive(true);
            initialText.text = "";

            foreach (char letter in initialTextChars)
            {
                typeWriterSound.Play();
                currentText += letter;
                initialText.text = currentText;      
                yield return new WaitForSeconds(0.1f);
            }

            initialText.gameObject.SetActive(false);
            headerText.gameObject.SetActive(true);
            yearText.gameObject.SetActive(true);
            bTypeWriterAnimationPlayed = true;
            bTypeWriterAnimationPlaying = false;
        }

        protected IEnumerator outroTextAnimation()
        {
            headerText.gameObject.SetActive(false);
            yearText.gameObject.SetActive(false);
            mainText.text = "";
            currentText = "";
            mainText.gameObject.SetActive(true);

            foreach (char letter1 in firstTextChars)
            {
                typeWriterSound.Play();
                currentText += letter1;
                mainText.text = currentText;      
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(2.0f);

            mainText.text = "";
            currentText = "";
            foreach (char letter2 in secondTextChars)
            {
                typeWriterSound.Play();
                currentText += letter2;
                mainText.text = currentText;      
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(1.5f);

            mainText.text = "";
            currentText = "";
            foreach (char letter3 in thirdTextChars)
            {
                typeWriterSound.Play();
                currentText += letter3;
                mainText.text = currentText;      
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(2.5f);

            fadeOutPanel.SetActive(true);

            yield return new WaitForSeconds(2.0f);

            bOutroAnimationPlayed = true;
            bOutroAnimationPlaying = false;
        }

        protected void simulateElapsedTime()
        {
            if (!bInitialElapsedAnimationPlayed) {
                
                //delay
                if (elapsedAnimationDelay > 0.0f) {
                    elapsedAnimationDelay -= Time.deltaTime;
                    return;
                } else {
                    if (! belapsedAnimationDelayCompleted) {
                        belapsedAnimationDelayCompleted = true;
                        timeSound.Play();
                    }
                }

                //adjust year
                if (! (currentYear >= targetYear - 1)) {
                    currentYear += yearStep;

                    //adjust speed
                    if (! (currentSpeed >= targetSpeed)) {
                        currentSpeed += speedStep;
                    }

                    //adjust camera position
                    if (! (currentCameraPos.y <= targetCameraPos.transform.position.y)) {
                        currentCameraPos.x = 0;
                        currentCameraPos.y += cameraYStep;
                        currentCameraPos.z += cameraZStep;
                        cameraContainer.transform.localPosition = currentCameraPos;
                    }

                    //adjust earth color
                    if (! (currentEarthColor.r >= targetEarthColor.r)) {
                        currentEarthColor.r += colorRStep;
                        currentEarthColor.g -= colorGStep;
                    }

                } else {
                    bInitialElapsedAnimationPlayed = true;
                }

            } else {
                currentSpeed = initialSpeed;

                if (flareDelayTime > 0.0f) {
                    flareDelayTime -= Time.deltaTime;
                } else {
                    bFinalElapsedAnimationPlayed = true;
                    sunAnimator.SetTrigger("flare");
                    mainCamera.SetActive(false);
                    secondaryCamera.SetActive(true);
                }
            }

            setEarthColor();
            setSpeed();
            setYear();
        }

        protected void fadeOutAtmosphere()
        {
            //delay
            if (atmosphereDelayTime > 0.0f) {
                atmosphereDelayTime -= Time.deltaTime;
                return;
            } else {
                if (! bAtmosphereSoundPlayed) {
                    bAtmosphereSoundPlayed = true;
                    atmosphereDieSound.Play();
                    mainCamera.SetActive(true);
                    secondaryCamera.SetActive(false);
                }
            }

            //adjust atmosphere color
            if (currentAtmosphereColor.a >= targetAtmosphereColor.a) {
                currentAtmosphereColor.a += colorAStep;
                setAtmosphereColor();
            } else {
                bAtmosphereAnimationPlayed = true;
            }
        }

        protected void setSpeed()
        {
            earthOrbit.speed = currentSpeed;
        }

        protected void setYear()
        {
            yearText.text = currentYear.ToString("00");
        }

        protected void setEarthColor()
        {
            EarthMaterial.SetColor("_BaseColor", currentEarthColor);
        }

        protected void setAtmosphereColor()
        {
            AtmosphereMaterial.SetColor("_BaseColor", currentAtmosphereColor);
        }
    }
}