using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class SnowBreakable : MonoBehaviour
    {
        // Desired duration of the shake effect
        protected float originalShakeDuration = 0.75f;
        protected float shakeDuration;
        // Time til shake
        protected float originalTimeTilShake = 0.25f;
        protected float timeTilShake;
        // A measure of magnitude for the shake. Tweak based on your preference
        protected float shakeMagnitude = 0.1f;
        // A measure of how quickly the shake effect should evaporate
        protected float dampingSpeed = 1.0f;
        // The initial position of the GameObject
        protected Vector3 initialPosition;
        //threshold distance
        protected float thresholdDistance = 0.75f;

        protected bool bBreak;
        protected bool bPlayerBrokeThreshold;

        void OnDisable()
        {
            transform.localPosition = initialPosition;
        }

        void OnEnable()
        {
            initialPosition = transform.localPosition;
            bBreak = false;
            bPlayerBrokeThreshold = false;
            shakeDuration = originalShakeDuration;
            timeTilShake = originalTimeTilShake;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player") {
                if (!bBreak) {
                    bBreak = true;
                }
            }
        }

        void Update()
        {
            //if player touched the game object
            if (bBreak) {

                //if player went higher than the game object at all
                if (! bPlayerBrokeThreshold) {
                    if (GameController.Instance.Player.transform.position.y + thresholdDistance > gameObject.transform.position.y) {
                        bPlayerBrokeThreshold = true;
                    }

                    return;
                }


                if (shakeDuration > 0) {

                    if (timeTilShake <= 0) {
                        transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
                        shakeDuration -= Time.deltaTime * dampingSpeed;
                    } else {
                        timeTilShake -= Time.deltaTime;
                    }

                } else {

                    shakeDuration = 0f;
                    transform.localPosition = initialPosition;
                    gameObject.SetActive(false);
                    
                }
            }
        }
    }

}