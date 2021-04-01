using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class CameraController : MonoBehaviour
    {
        public Camera cam;
        protected Transform target;                // The position that that camera will be following.
        protected float smoothing = 5f;            // The speed with which the camera will be following.
        protected Vector3 offset;                         // The initial offset from the target.
        protected float zoomOutDistance = 5f;
        protected float zoomOutStep;
        protected float targetDistance;


        // Desired duration of the shake effect
        protected float originalShakeDuration = 2.0f;
        protected float shakeDuration;
        // Time til shake
        protected float originalTimeTilShake = 0.1f;
        protected float timeTilShake;
        // A measure of magnitude for the shake. Tweak based on your preference
        protected float shakeMagnitude = 0.1f;
        // A measure of how quickly the shake effect should evaporate
        protected float dampingSpeed = 1.0f;
        // The initial position of the GameObject
        protected Vector3 initialPosition;
        //if shake
        public bool bShake;


        void Start ()
        {
            //calculate zoom out time
            zoomOutStep = (zoomOutDistance / 60.0f) / 2.0f;
            targetDistance = cam.fieldOfView + zoomOutDistance;

            target = GameController.Instance.Player.transform;
            // Calculate the initial offset.
            offset = transform.position - target.position;

            initialPosition = transform.position;    
            resetShake();
        }


        void Update ()
        {
            if (GameController.Instance.isGameActive) {
                //zoom out
                if (! (cam.fieldOfView >= targetDistance)) {
                    cam.fieldOfView += zoomOutStep;
                }

                // Create a postion the camera is aiming for based on the offset from the target.
                Vector3 targetCamPos = target.position + offset;
                targetCamPos.x = gameObject.transform.position.x;
                targetCamPos.z = gameObject.transform.position.z;

                // Smoothly interpolate between the camera's current position and it's target position.
                transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
            }


            if (bShake) {
                if (shakeDuration > 0) {

                    if (timeTilShake <= 0) {
                        transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
                        shakeDuration -= Time.deltaTime * dampingSpeed;
                    } else {
                        timeTilShake -= Time.deltaTime;
                    }

                } else {
                    resetShake();
                }
            }
        }

        public Vector3 getOffset()
        {
            return offset;
        }

        public void Shake()
        {
            bShake = true;
        }

        protected void resetShake()
        {
            bShake = false;
            transform.position = initialPosition;
            shakeDuration = originalShakeDuration;
            timeTilShake = originalTimeTilShake;
        }
    }
}