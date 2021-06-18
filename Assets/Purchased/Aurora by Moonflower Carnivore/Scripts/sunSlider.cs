using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class sunSlider : MonoBehaviour {
	public Slider mainSlider;
	public void sun () {
		float value2 = (mainSlider.value) * 2F;
		if (mainSlider.value>0.5) {//when slider value is greater than 1, value 2 pingpong backward instead of growing greater.
			value2 = (1F-value2)*2F+value2;
		}
		Light light = GetComponent<Light> ();
		light.intensity = value2*2F;
	}
}