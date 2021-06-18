using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftChallengeDialogText : DialogText
{
    public GameObject LeftIndicator;
    protected float timer= 3.0f;
    public Text timerText;
    protected int currentTime;

    public void Start()
    {
        currentTime = (int) timer;
    }

    public override IEnumerator WaitForCondition()
    {
        yield return new WaitUntil(() => Timer());
    }

    public bool Timer()
    {
        if (currentTime <= 0) {
            return true;
        } else {
            if (JoyStickHandle.HorizontalState == JoyStickHandle.HorizontalStateEnum.LA) {
                timer -= Time.deltaTime;
                if ((int) timer < currentTime) {
                    currentTime = (int) timer;
                    timerText.text = currentTime.ToString() + " seconds";
                }
            }
        }

        return false;
    }
}