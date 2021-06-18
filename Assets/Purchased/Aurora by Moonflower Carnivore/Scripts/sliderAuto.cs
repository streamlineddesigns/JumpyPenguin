using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class sliderAuto : MonoBehaviour {
	private Slider slider;
	void OnEnable () {
		slider = GetComponent<Slider>();
		InvokeRepeating("slide", 0f, 0.0167f);
	}
	void OnDisable(){
		CancelInvoke();
	}
	void slide() {
		slider.value+=0.00025f;
		if (slider.value==1f) {
			slider.value = 0f;
		}
	}
}