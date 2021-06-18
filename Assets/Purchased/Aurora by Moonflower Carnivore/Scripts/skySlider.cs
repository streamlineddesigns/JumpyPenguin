using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class skySlider : MonoBehaviour {
	//public GameObject cloud; //"particle slave cloud" of "Cloud 000"
	public Slider mainSlider;
	public void sky () {
		//ParticleSystem psCloud = cloud.GetComponent<ParticleSystem> ();
		//ParticleSystem.MainModule main = psCloud.main;
		//main.startColor = new Color(mainSlider.value,mainSlider.value,mainSlider.value,1F);
		this.transform.localEulerAngles=new Vector3 ((mainSlider.value*360)-90,-90,0);
	}
}