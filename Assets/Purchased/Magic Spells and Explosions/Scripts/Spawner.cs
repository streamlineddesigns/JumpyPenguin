using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public GameObject pref_Obj;
	public AnimationCurve curveInterval;

	private int spawnedObjCount = 0;
	private float tempTime = 0.0f;
	private float time = 0f;

	// Use this for initialization
	void Start ()
	{
		tempTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
	{
		time += Time.deltaTime;

		if (curveInterval [curveInterval.length - 1].time < time)
			return;

		if(Time.time - tempTime >= curveInterval.Evaluate(time))
		{
			tempTime = Time.time;
			Instantiate(pref_Obj, transform.position, pref_Obj.transform.rotation);
		}
	}
}
