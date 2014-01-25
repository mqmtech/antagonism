using UnityEngine;
using System.Collections;

public class Action : MonoBehaviour {

	Vector2 originalPosition;
	float returnVelocity = 10;

	public float maxVel = 1.5f;

	float accel = 0f;

	bool pushed = false;

	void Start() {
		originalPosition = new Vector2(transform.position.x, transform.position.y);
	}


	// Use this for initialization
	void onMotionActivate (Vector2 velocity) {
		rigidbody2D.velocity += velocity;

		float vel = Mathf.Clamp(rigidbody2D.velocity.magnitude, -maxVel, maxVel);
		rigidbody2D.velocity.Normalize ();
		rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x * vel, rigidbody2D.velocity.y *vel);

		Debug.Log (velocity);
	}
	
	void Update () {
		Vector2 position = new Vector2(transform.position.x, transform.position.y);
		//Debug.Log ("AAAAAAAAAAA");
		//Debug.Log (originalPosition);
		//Debug.Log (position);
		float dist = Vector2.Distance(originalPosition, position);
		if (dist > 0.1) {

			Vector3 difVec = (originalPosition - position);
			float diffModule = difVec.sqrMagnitude;
			accel += diffModule;

			rigidbody2D.velocity += (originalPosition - position).normalized * accel * Time.deltaTime;

			//Debug.Log (rigidbody2D.velocity);

			/*
			Vector2 opositeForce = (originalPosition - position);
			rigidbody2D.velocity += opositeForce*returnVelocity*0.5f;
			*/

		}   
		else {
		}
	}

}
