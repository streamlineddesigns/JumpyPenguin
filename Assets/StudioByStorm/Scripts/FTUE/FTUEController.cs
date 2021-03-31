using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StudioByStorm.Scripts.FTUE
{
    public class FTUEController : MonoBehaviour
    {
        public RotateSimple earthOrbit;
        public Material EarthMaterial;
        protected float colorRStep;
        protected float colorGStep;
        protected float timeTilColor = 5.0f;
        protected Color currentEarthColor;
        protected Color initialEarthColor = new Color(0.02f, 0.54f, 0f);
        protected Color targetEarthColor = new Color(0.79f, 0.49f, 0f);

        public Text initialText;
        public Text headerText;
        public Text yearText;

        protected bool bTypeWriterAnimationPlayed = false;
        protected bool bInitialElapsedAnimationPlayed = false;
        protected bool bFinalElapsedAnimationPlayed = false;

        protected float speedStep;
        protected float timeTilMaxSpeed = 2.0f;
        protected float currentSpeed;
        protected float initialSpeed = 10;
        protected float targetSpeed = 1000;

        protected float yearStep;
        protected float timeTilMaxYear = 5.0f;
        protected float currentYear;
        protected float initialYear = 1954;
        protected float targetYear = 2154;

        protected float cameraYStep;
        protected float cameraZStep;
        protected float timeTilTargetCameraPos = 5.0f;
        public GameObject cameraContainer;
        protected Vector3 currentCameraPos;
        public GameObject initialCameraPos;
        public GameObject targetCameraPos;

        public Animator sunAnimator;

        // Start is called before the first frame update
        void Start()
        {
            currentEarthColor = initialEarthColor;
            colorRStep = (targetEarthColor.r - initialEarthColor.r) / (60.0f * timeTilColor);
            colorGStep = (targetEarthColor.g - initialEarthColor.g) / (60.0f * timeTilColor);

            currentSpeed = initialSpeed;
            currentYear = initialYear;

            speedStep = (targetSpeed - initialSpeed) / (60.0f * timeTilMaxSpeed);
            yearStep = (targetYear - initialYear) / (60.0f * timeTilMaxYear);

            currentCameraPos = cameraContainer.transform.position;
            cameraYStep = (targetCameraPos.transform.position.y - initialCameraPos.transform.position.y) / (60.0f * timeTilTargetCameraPos);
            cameraZStep = (targetCameraPos.transform.position.z - initialCameraPos.transform.position.z) / (60.0f * timeTilTargetCameraPos);
        }

        // Update is called once per frame
        void Update()
        {
            if (!bTypeWriterAnimationPlayed) {
                //return;
            }
            
            if (! bFinalElapsedAnimationPlayed) {
                simulateElapsedTime();
            }
        }

        protected void typeWriterAnimation()
        {

        }

        protected void simulateElapsedTime()
        {
            if (!bInitialElapsedAnimationPlayed) {
                
                //adjust year
                if (! (currentYear >= targetYear)) {
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

                    //adjust color
                    if (! (currentEarthColor.r >= targetEarthColor.r)) {
                        currentEarthColor.r += colorRStep;
                        currentEarthColor.g -= colorGStep;
                    }

                } else {
                    bInitialElapsedAnimationPlayed = true;
                }

            } else {
                currentSpeed = initialSpeed;
                bFinalElapsedAnimationPlayed = true;
                sunAnimator.SetTrigger("flare");
            }

            setColor();
            setSpeed();
            setYear();
        }

        protected void setSpeed()
        {
            earthOrbit.speed = currentSpeed;
        }

        protected void setYear()
        {
            yearText.text = currentYear.ToString("00");
        }

        protected void setColor()
        {
            EarthMaterial.SetColor("_BaseColor", currentEarthColor);
        }
    }
}