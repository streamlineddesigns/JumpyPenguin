using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class starSlider : MonoBehaviour {
	public Slider mainSlider;
	public float maxSize=0.001F;
	public void star () {
		float value2 = (mainSlider.value) * 2F;
		if (mainSlider.value>0.5) {//when slider value is greater than 1, value 2 pingpong backward instead of growing greater.
			value2 = (1F-value2)*2F+value2;
		}
		ParticleSystemRenderer psStar = GetComponent<ParticleSystemRenderer> ();
		psStar.maxParticleSize = maxSize / value2 - 0.001F;
	}
}