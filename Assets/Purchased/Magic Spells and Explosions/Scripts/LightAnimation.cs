using UnityEngine;
using System.Collections;

public class LightAnimation : MonoBehaviour
{
	public AnimationCurve lightIntenseCurve;
	public float          m_DefaultIntensity;
	public bool           disableOnAnimEnd;
	public bool           m_LoopAnimation = false;
	private float         lightIntenseAnimTime = 0.0f;
	public float delay = 0.0f;

	private Light m_Light;

	// Use this for initialization
	void Awake()
	{
		m_Light = GetComponent<Light>();

		if(delay > 0)
		{
			m_Light.enabled = false;
			Invoke("Activate", delay);
		}
	}

	void Activate()
	{
		m_Light.enabled = true;
	}
	
	// Update is called once per frame
	void Update()
	{
		if(!m_Light.enabled)
			return;

		if(lightIntenseAnimTime <= lightIntenseCurve[lightIntenseCurve.length - 1].time)
		{
			GetComponent<Light>().intensity = lightIntenseCurve.Evaluate(lightIntenseAnimTime);
			lightIntenseAnimTime += Time.deltaTime;
		}
		else if(lightIntenseAnimTime > lightIntenseCurve[lightIntenseCurve.length - 1].time && m_LoopAnimation) 
		{
			GetComponent<Light>().intensity = m_DefaultIntensity;
			lightIntenseAnimTime = 0;
		}
		else if(disableOnAnimEnd)
			GetComponent<Light>().enabled = false;
	}
}
