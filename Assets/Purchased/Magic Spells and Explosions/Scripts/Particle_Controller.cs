using UnityEngine;
using System.Collections;

public class Particle_Controller : MonoBehaviour
{

	// The name of the sorting layer the particles should be set to.
	public string sortingLayerName;		
	public bool destoryAfterEndPlay = false;
	
	void Start()
	{
		
	}
	
	void Update()
	{
		if(GetComponent<AudioSource>())
		{
			if(GetComponent<AudioSource>().isPlaying)
				return;
		}

		if(destoryAfterEndPlay)
		{
			if(!ParticlePlaying(transform))
			{
				Destroy(gameObject);
			}
		}
	}

	bool ParticlePlaying(Transform p_Transform)
	{
		if(p_Transform.GetComponent<ParticleSystem>())
		{
			if(p_Transform.GetComponent<ParticleSystem>().isPlaying)
				return true;
		}
		
		for(int i = 0; i < p_Transform.childCount; i++)
		{
			if(ParticlePlaying(p_Transform.GetChild(i)))
			{
				return true;
			}
		}
		
		return false;
	}
}
