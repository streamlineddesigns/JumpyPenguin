using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class timeSlider : MonoBehaviour {
	public Slider mainSlider;
	public void time () {
		float hour = Mathf.Floor(mainSlider.value * 24F);
		string apm = " am";
		if (hour>=12) {
			apm = " pm";
		}
		string hourc = hour.ToString();
		if (hour>=13) {
			hour=hour-12F;
			hourc=hour.ToString();
		}
		if (hour<10) {
			hourc=("0"+hour.ToString());
		}
		float minute = Mathf.Floor((mainSlider.value * 24F - Mathf.Floor(mainSlider.value * 24F))*60);
		string minutec = minute.ToString();
		if (minute<10) {
			minutec=("0"+minute.ToString());
		}
		string format = System.String.Format("Time "+hourc+":"+minutec+apm);
		GetComponent<Text>().text = format;
	}
}