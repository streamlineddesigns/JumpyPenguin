using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class auroraTextureButton : MonoBehaviour {
	public GameObject slaveObject;
	public GameObject slaveObject2;
	public GameObject slaveObject3;
	private ParticleSystem ps;
	private ParticleSystem ps2;
	private ParticleSystem ps3;
	public GameObject lightObject;
	private ParticleSystem.TextureSheetAnimationModule tsa;
	private ParticleSystem.TextureSheetAnimationModule tsa2;
	private ParticleSystem.TextureSheetAnimationModule tsa3;
	private Light dlight;
	void Start () {
		ps = slaveObject.GetComponent<ParticleSystem> ();
		ps2 = slaveObject2.GetComponent<ParticleSystem> ();
		ps3 = slaveObject3.GetComponent<ParticleSystem> ();
		tsa = ps.textureSheetAnimation;
		tsa2 = ps2.textureSheetAnimation;
		tsa3 = ps3.textureSheetAnimation;
		dlight = lightObject.GetComponent<Light> ();
	}
	
	void Update () {
		if (Input.GetKeyDown("z")) {
				green ();
				greenLight ();
		} else if (Input.GetKeyDown("x")) {
				palegreen ();
				palegreenLight ();
		} else if (Input.GetKeyDown("c")) {
				red ();
				redLight ();
		} else if (Input.GetKeyDown("v")) {
				blue ();
				blueLight ();
		}
	}
	
	public void green () {
		tsa.frameOverTimeMultiplier = 0f;
		tsa2.frameOverTimeMultiplier = 0f;
		tsa3.frameOverTimeMultiplier = 0f;
	}
	public void greenLight () {
		dlight.color = new Color(0.145F,1F,0.07F,1F);
	}
	public void palegreen () {
		tsa.frameOverTimeMultiplier = 1/4f;
		tsa2.frameOverTimeMultiplier = 1/4f;
		tsa3.frameOverTimeMultiplier = 1/4f;
	}
	public void palegreenLight () {
		dlight.color = new Color(0.545F,1F,0.47F,1F);
	}
	public void red () {
		tsa.frameOverTimeMultiplier = 2/4f;
		tsa2.frameOverTimeMultiplier = 2/4f;
		tsa3.frameOverTimeMultiplier = 2/4f;
	}
	public void redLight () {
		dlight.color = new Color(1F,0.545F,0.47F,1F);
	}
	public void blue () {
		tsa.frameOverTimeMultiplier = 3/4f;
		tsa2.frameOverTimeMultiplier = 3/4f;
		tsa3.frameOverTimeMultiplier = 3/4f;
	}
	public void blueLight () {
		dlight.color = new Color(0.47F,0.545F,1F,1F);
	}
}