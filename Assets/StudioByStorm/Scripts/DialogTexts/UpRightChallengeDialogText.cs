using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpRightChallengeDialogText : DialogText
{
    protected int countNeeded = 3;
    public Text countText;
    protected bool bCanUpdate;

    public override IEnumerator WaitForCondition()
    {
        yield return new WaitUntil(() => Counter());
    }

    public bool Counter()
    {
        if (countNeeded <= 0) {
            return true;
        } else {
            if (JoyStickHandle.HorizontalState == JoyStickHandle.HorizontalStateEnum.RA && JoyStickHandle.VerticalState == JoyStickHandle.VerticalStateEnum.ON) {
                
                if (bCanUpdate) {
                    bCanUpdate = false;
                    countNeeded--;
                    countText.text = countNeeded.ToString() + " times";
                }
  
            } else {
                bCanUpdate = true;
            }
        }

        return false;
    }
}