using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnParticles : MonoBehaviour
{
	public List<GameObject> particles;
	
	private int index = 0;
	private GameObject currentParticle;

	// Use this for initialization
	void Start()
	{
		if(particles[index])
			currentParticle = Instantiate(particles[index], particles[index].transform.position, particles[index].transform.rotation) as GameObject;
	}



	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			index++;
			
			if(index > particles.Count - 1)
				index = 0;

			if(currentParticle)
			{
				Destroy(currentParticle);
				currentParticle = null;
			}

			if(particles[index])
				currentParticle = Instantiate(particles[index], particles[index].transform.position, particles[index].transform.rotation) as GameObject;
		}
		else if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			index--;
			
			if(index < 0)
				index = particles.Count - 1;

			if(currentParticle)
			{
				Destroy(currentParticle);
				currentParticle = null;
			}

			if(particles[index])
				currentParticle = Instantiate(particles[index], particles[index].transform.position, particles[index].transform.rotation) as GameObject;
		}
	}
}
