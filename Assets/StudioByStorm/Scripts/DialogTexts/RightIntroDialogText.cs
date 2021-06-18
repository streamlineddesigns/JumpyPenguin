using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightIntroDialogText : DialogText
{
    public BoxCollider2D RightArrow;
    public GameObject RightIndicator;

    public override IEnumerator WaitForCondition()
    {
        RightArrow.enabled = true;
        RightIndicator.SetActive(true);

        yield return new WaitUntil(() => JoyStickHandle.HorizontalState == JoyStickHandle.HorizontalStateEnum.RA);

        RightIndicator.SetActive(false);
        
        yield return new WaitForSeconds(3.0f);
    }
}