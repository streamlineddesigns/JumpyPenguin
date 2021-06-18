using UnityEngine;
using System.Collections;

public class auroraLightFade : MonoBehaviour {
	[Tooltip("If Target Intensity is zero, it will assume the Intensity value in the Light component.")]
	public float targetIntensity = 0.15f;
	[Tooltip("Increment or decrement value of Intensity per frame.")]
	public float stepPerFrame = 0.00025f;
	private Light dLight;
	public GameObject auroraMaster1;
	public GameObject auroraMaster2;
	void Start () {
		dLight = GetComponent<Light> ();
		if (targetIntensity == 0f) {
			targetIntensity = dLight.intensity;
		}
		dLight.intensity = 0f;
	}
	void Update () {
		if (!auroraMaster1.activeSelf && !auroraMaster2.activeSelf) {
			StartCoroutine(fadeout());
		} else if ((auroraMaster1.activeSelf || auroraMaster2.activeSelf) && dLight.intensity < targetIntensity) {
			StartCoroutine(fadein());
		}
	}
	IEnumerator fadeout(){
		while (dLight.intensity > 0f) {
			dLight.intensity -= stepPerFrame;
			yield return null;
		}
	}
	IEnumerator fadein(){
		while (dLight.intensity < targetIntensity) {
			dLight.intensity += stepPerFrame;
			yield return null;
		}
	}
}