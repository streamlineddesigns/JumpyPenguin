using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpIntroDialogText : DialogText
{
    public BoxCollider2D UpArrow;
    public GameObject UpIndicator;

    public override IEnumerator WaitForCondition()
    {
        UpArrow.enabled = true;
        UpIndicator.SetActive(true);

        yield return new WaitUntil(() => JoyStickHandle.VerticalState == JoyStickHandle.VerticalStateEnum.ON);

        UpIndicator.SetActive(false);

        yield return new WaitForSeconds(3.0f);
    }
}