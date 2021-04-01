using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts.FTUE
{
    public class IntroCamera : MonoBehaviour
    {
        public Transform target;
        protected float movementDelayTime = 4.0f;
        protected bool bPlayedEarthAnimation;
        public Animator earthAnimator;
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        void Update()
        {
            //rotate
            Vector3 relativePos = target.position - transform.position;
            Quaternion LookAtRotation = Quaternion.LookRotation(relativePos, Vector3.up);
            Quaternion LookAtRotationOnly_Y = Quaternion.Euler(transform.rotation.eulerAngles.x, LookAtRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = LookAtRotationOnly_Y;
            
            //move
            if (movementDelayTime > 0.0f) {
                movementDelayTime -= Time.deltaTime;
            } else {
                transform.position = Vector3.Lerp(transform.position, target.position, 0.1f);
                if (!bPlayedEarthAnimation) {
                    bPlayedEarthAnimation = true;
                    earthAnimator.SetTrigger("pulse");
                }
            }
        }
    }
}