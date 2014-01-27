using UnityEngine;
using System.Collections;

public class WindParticles : MonoBehaviour
{
	public Transform parentTransform;

	Vector3 deltaPos = new Vector3();
	Vector3 originalPos = new Vector3();

	ParticleSystem particleSystem;

	public float maxForce = 1f;

	// Use this for initialization
	void Start () {
		originalPos = transform.position;
		deltaPos = transform.position - parentTransform.position;
		particleSystem = transform.GetChild (0).particleSystem;
	}
	
	void Update () {
		updatePos ();
	}

	void FixedUpdate() {
		updatePos ();
	}

	void OnTriggerEnter(Collider otherCollider)
	{
		if (otherCollider.tag != "FingerTip") return;

		Transform other = otherCollider.gameObject.transform;

		if (other.parent != null) {
			other = other.parent.transform;
		}

		Vector3 vel = other.rigidbody.velocity;

		Quaternion newRotation = Quaternion.LookRotation (vel.normalized);
		transform.rotation = Quaternion.Lerp (transform.rotation, newRotation, Time.deltaTime);

		changeParticleVel (vel);
	}


	void changeParticleVel (Vector3 vel) {

		ParticleSystem.Particle[] p = new ParticleSystem.Particle[particleSystem.particleCount+1];
		int l = particleSystem.GetParticles(p);
		
		int i = 0;
		while (i < l) {
			//p[i].velocity = new Vector3(0, p[i].lifetime / p[i].startLifetime * 10F, 0);

			float force = Random.Range(0f, 3f);
			p[i].velocity += vel * Time.deltaTime * force * maxForce;
			i++;
		}
		
		particleSystem.SetParticles(p, l); 
	}



	void updatePos()
	{
		transform.position = parentTransform.position + deltaPos;

	}
}
