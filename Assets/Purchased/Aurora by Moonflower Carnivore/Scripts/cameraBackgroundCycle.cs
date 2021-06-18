using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class cameraBackgroundCycle : MonoBehaviour {
	public Slider mainSlider;
	public void backgroundSlider () {
		float value2 = ((mainSlider.value) * 2F - 0.0F) * 1.0F;
		if (mainSlider.value>0.5) {//when slider value is greater than 1, value 2 pingpong backward instead of growing greater.
			value2 = (1F-value2)*2.0F+value2;
		}
		GetComponent<Camera>().backgroundColor = new Color((value2-0.4F)*1.45F,(value2-0.5F)*1.85F,(value2-0.55F)*2.3F,0F);
	}
}