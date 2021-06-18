using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class auroraMaterialButton : MonoBehaviour {
	public Material psrm;
	private ParticleSystemRenderer psr;
	
	void Update () {
		if (Input.GetKeyDown("z")) {
				green ();
		} else if (Input.GetKeyDown("x")) {
				palegreen ();
		} else if (Input.GetKeyDown("c")) {
				red ();
		} else if (Input.GetKeyDown("v")) {
				blue ();
		}
	}
	
	public void green () {
		psrm.SetColor("_ColorTop", new Color(0.86F, 0F, 1F, 1F));
		psrm.SetColor("_ColorMid", new Color(0F, 1F, 0.09F, 1F));
		psrm.SetColor("_ColorBot", new Color(0.28F, 1F, 0F, 1F));
	}
	public void palegreen () {
		psrm.SetColor("_ColorTop", new Color(0.157F, 1F, 0F, 1F));
		psrm.SetColor("_ColorMid", new Color(0.15F, 1F, 0.3F, 1F));
		psrm.SetColor("_ColorBot", new Color(0.59F, 1F, 0.85F, 1F));
	}
	public void red () {
		psrm.SetColor("_ColorTop", new Color(1F, 0F, 0F, 1F));
		psrm.SetColor("_ColorMid", new Color(1F, 0.24F, 0.07F, 1F));
		psrm.SetColor("_ColorBot", new Color(1F, 0.87F, 0.18F, 1F));
	}
	public void blue () {
		psrm.SetColor("_ColorTop", new Color(0F, 0.04F, 1F, 1F));
		psrm.SetColor("_ColorMid", new Color(0.1F, 0.41F, 1F, 1F));
		psrm.SetColor("_ColorBot", new Color(0.62F, 0.91F, 1F, 1F));
	}
	void OnDisable () {
		green ();
	}
}