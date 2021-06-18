using UnityEngine;
using System.Collections;

public class particleLightButton : MonoBehaviour {
	public void clickOn () {
		ParticleSystem ps = GetComponent<ParticleSystem> ();
		ParticleSystem.LightsModule light = ps.lights;
		light.enabled = true;
	}
	public void clickOff () {
		ParticleSystem ps = GetComponent<ParticleSystem> ();
		ParticleSystem.LightsModule light = ps.lights;
		light.enabled = false;
	}
}