using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleManager : MonoBehaviour {

	static List<ParticleSystem> particles = new List<ParticleSystem>();

	public static void Instantiate(ParticleSystem particleSystem)
	{
		Instantiate (particleSystem.gameObject);
		ParticleManager.particles.Add (particleSystem);
	}

	// Update is called once per frame
	void Update () {
		foreach (ParticleSystem particle  in ParticleManager.particles) {
			if(particle.IsAlive() == false) {
				Destroy(particle);
			}
		}
	}
}
