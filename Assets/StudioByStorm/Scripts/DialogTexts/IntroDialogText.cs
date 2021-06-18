using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudioByStorm.Scripts
{
    public class IntroDialogText : DialogText
    {
        public float Timer = 3.0f;

        public override IEnumerator WaitForCondition()
        {
            yield return new WaitUntil(() => GameController.Instance.UserInterfaceController.isDialogBeingClicked || TimeElapsed());
        }

        public bool TimeElapsed()
        {
            if (Timer > 0.0f) {
                Timer -= Time.deltaTime;
            } else {
                return true;
            }

            return false;
        }
    }
}