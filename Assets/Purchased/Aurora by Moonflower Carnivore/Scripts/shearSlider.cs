using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class shearSlider : MonoBehaviour {
	public Slider mainSlider;
	public Material psrm;
	public void SliderF () {
		psrm.SetFloat("_Shear", mainSlider.value * 4F - 2F);
	}
}