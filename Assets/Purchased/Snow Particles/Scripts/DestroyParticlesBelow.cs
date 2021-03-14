#pragma warning disable 0618
using UnityEngine;
public class DestroyParticlesBelow : MonoBehaviour {

	public ParticleSystem m_System;
	ParticleSystem.Particle[] m_Particles;
	[Tooltip("Destroys all particles below Y position")]
	public float destroyBelow = 0f;

	private void Start() {
		Init();
	}

	private void Init() {
		m_System = GetComponent<ParticleSystem>();
		m_Particles = new ParticleSystem.Particle[m_System.maxParticles];
		InvokeRepeating("DestroyParticles", 0, 0.1f);
	}

	void DestroyParticles() {
		int numParticlesAlive = m_System.GetParticles(m_Particles);
		for (int i = 0; i < numParticlesAlive; i++) {
			
			if (m_Particles[i].position.y < destroyBelow) {
				m_Particles[i].lifetime = -1;
			}
		}
		m_System.SetParticles(m_Particles, numParticlesAlive);
	}

#if UNITY_EDITOR
	private void OnDrawGizmosSelected() {
		CancelInvoke();
		Init();
	}
#endif

}