using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class effectLabel : MonoBehaviour {
	private ParticleSystem Agitate;
	private ParticleSystem Aim;
	private ParticleSystem Alert;
	private ParticleSystem Stimulate;
	private ParticleSystem Fortify;
	private ParticleSystem Enlighten;
	private ParticleSystem Determine;
	private ParticleSystem Repel;
	private ParticleSystem Metabolize;
	private ParticleSystem Purify;
	private ParticleSystem Harmonize;
	private ParticleSystem MendI;
	private ParticleSystem MendII;
	private ParticleSystem MendIII;
	private ParticleSystem PanMendI;
	private ParticleSystem PanMendII;
	private ParticleSystem PanMendIII;
	private Image image;
	private Text text;
	private float time = 0f;
	private string prev = "";
	private float timeBreak = 30f;
	private Color color;
	void Start () {
		Agitate = GameObject.Find("magic Agitate container").GetComponent<ParticleSystem>();
		Aim = GameObject.Find("magic Aim container").GetComponent<ParticleSystem>();
		Alert = GameObject.Find("magic Alert container").GetComponent<ParticleSystem>();
		Stimulate = GameObject.Find("magic Stimulate container").GetComponent<ParticleSystem>();
		Fortify = GameObject.Find("magic Fortify container").GetComponent<ParticleSystem>();
		Enlighten = GameObject.Find("magic Enlighten container").GetComponent<ParticleSystem>();
		Determine = GameObject.Find("magic Determine container").GetComponent<ParticleSystem>();
		Repel = GameObject.Find("magic Repel container").GetComponent<ParticleSystem>();
		Metabolize = GameObject.Find("magic Metabolize container").GetComponent<ParticleSystem>();
		Purify = GameObject.Find("magic Purify container").GetComponent<ParticleSystem>();
		Harmonize = GameObject.Find("magic Harmonize container").GetComponent<ParticleSystem>();
		MendI = GameObject.Find("magic Mend I container").GetComponent<ParticleSystem>();
		MendII = GameObject.Find("magic Mend II container").GetComponent<ParticleSystem>();
		MendIII = GameObject.Find("magic Mend III container").GetComponent<ParticleSystem>();
		PanMendI = GameObject.Find("magic Panmend I container").GetComponent<ParticleSystem>();
		PanMendII = GameObject.Find("magic Panmend II container").GetComponent<ParticleSystem>();
		PanMendIII = GameObject.Find("magic Panmend III container").GetComponent<ParticleSystem>();
		
		image = GetComponent<Image>();
		text = GameObject.Find("Text skill").GetComponent<Text>();
	}
	void Update () {
		if (time >= 120f) {
			time = 0f;
		} else if (time >= 60f) {
			time++;
			labelState("",false);
			prev = "";
		} else if (prev == "Agitate" && time >= timeBreak) {
			time++;
			labelState("Agility improved!",true);
			color = Color.yellow;
		} else if (Agitate.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Agitate";
			labelState(prev,true);
			color = Color.white;
		} else if (prev == "Aim" && time >= timeBreak) {
			time++;
			labelState("Accuracy improved!",true);
			color = Color.yellow;
		} else if (Aim.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Aim";
			labelState(prev,true);
			color = Color.white;
		} else if (prev == "Alert" && time >= timeBreak) {
			time++;
			labelState("Evasion improved!",true);
			color = Color.yellow;
		} else if (Alert.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Alert";
			labelState(prev,true);
			color = Color.white;
		} else if (prev == "Repel" && time >= timeBreak) {
			time++;
			labelState("Shield erected!",true);
			color = Color.yellow;
		} else if (Repel.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Repel";
			labelState(prev,true);
			color = Color.white;
		} else if (prev == "Stimulate" && time >= timeBreak) {
			time++;
			labelState("Strength improved!",true);
			color = Color.yellow;
		} else if (Stimulate.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Stimulate";
			labelState(prev,true);
			color = Color.white;
		} else if (prev == "Fortify" && time >= timeBreak) {
			time++;
			labelState("Defense improved!",true);
			color = Color.yellow;
		} else if (Fortify.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Fortify";
			labelState(prev,true);
			color = Color.white;
		} else if (prev == "Enlighten" && time >= timeBreak) {
			time++;
			labelState("Intelligence improved!",true);
			color = Color.yellow;
		} else if (Enlighten.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Enlighten";
			labelState(prev,true);
			color = Color.white;
		} else if (prev == "Determine" && time >= timeBreak) {
			time++;
			labelState("Mind improved!",true);
			color = Color.yellow;
		} else if (Determine.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Determine";
			labelState(prev,true);
			color = Color.white;
		} else if (prev == "Metabolize" && time >= timeBreak) {
			time++;
			labelState("Health regenerates!",true);
			color = Color.yellow;
		} else if (Metabolize.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Metabolize";
			labelState(prev,true);
			color = Color.white;
		} else if (prev == "Purify" && time >= timeBreak) {
			time++;
			labelState("Ailments gone!",true);
			color = Color.yellow;
		} else if (Purify.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Purify";
			labelState(prev,true);
			color = Color.white;
		} else if (prev == "Harmonize" && time >= timeBreak) {
			time++;
			labelState("Buffs negated!",true);
			color = Color.yellow;
		} else if (Harmonize.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Harmonize";
			labelState(prev,true);
			color = Color.white;
		} else if ((prev == "Mend I" || prev == "Mend II" || prev == "Mend III" || prev == "Panmend I" || prev == "Panmend II" || prev == "Panmend III") && time >= timeBreak) {
			time++;
			labelState("Health restored!",true);
			color = Color.yellow;
		} else if (MendI.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Mend I";
			labelState(prev,true);
			color = Color.white;
		} else if (MendII.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Mend II";
			labelState(prev,true);
			color = Color.white;
		} else if (MendIII.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Mend III";
			labelState(prev,true);
			color = Color.white;
		} else if (PanMendI.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Panmend I";
			labelState(prev,true);
			color = Color.white;
		} else if (PanMendII.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Panmend II";
			labelState(prev,true);
			color = Color.white;
		} else if (PanMendIII.IsAlive(true) && time < timeBreak) {
			time++;
			prev = "Panmend III";
			labelState(prev,true);
			color = Color.white;
		} else {
			time = 0f;
			labelState("",false);
			prev = "";
		}
		//Debug.Log(time);
	}
	void labelState (string name,bool state) {
		text.text = name;
		image.enabled = state;
		text.enabled = state;
		text.color = color;
	}
}