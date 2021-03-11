using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class CameraController : MonoBehaviour
    {
        protected Transform target;                // The position that that camera will be following.
        protected float smoothing = 5f;            // The speed with which the camera will be following.
        Vector3 offset;                         // The initial offset from the target.


        void Start ()
        {
            target = GameController.Instance.Player.transform;
            // Calculate the initial offset.
            offset = transform.position - target.position;
        }


        void Update ()
        {
            if (GameController.Instance.isGameActive) {
                // Create a postion the camera is aiming for based on the offset from the target.
                Vector3 targetCamPos = target.position + offset;
                targetCamPos.x = gameObject.transform.position.x;
                targetCamPos.z = gameObject.transform.position.z;

                // Smoothly interpolate between the camera's current position and it's target position.
                transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
            }
        }

        public Vector3 getOffset()
        {
            return offset;
        }
    }
}