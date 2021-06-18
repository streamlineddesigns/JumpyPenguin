using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftIntroDialogText : DialogText
{
    public BoxCollider2D LeftArrow;
    public GameObject LeftIndicator;

    public override IEnumerator WaitForCondition()
    {
        LeftArrow.enabled = true;
        LeftIndicator.SetActive(true);

        yield return new WaitUntil(() => JoyStickHandle.HorizontalState == JoyStickHandle.HorizontalStateEnum.LA);

        LeftIndicator.SetActive(false);

        yield return new WaitForSeconds(3.0f);
    }
}