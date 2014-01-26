using UnityEngine;
using System.Collections;

public class Action : MonoBehaviour {

	private Vector2 originalPosition;
	public float returnVelocity = 10;
	public float maxVel = 15.0f;

	void Start() {
		originalPosition = new Vector2(transform.position.x, transform.position.y);
	}


	// Use this for initialization
	void onMotionActivate (Vector2 velocity) {
		rigidbody2D.velocity += velocity;

		float vel = Mathf.Clamp(rigidbody2D.velocity.magnitude, -maxVel, maxVel);
		rigidbody2D.velocity.Normalize ();
		rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x * vel, rigidbody2D.velocity.y *vel);

		//Debug.Log (velocity);
	}
	
	void Update () {
		Vector2 position = new Vector2(transform.position.x, transform.position.y);
		//Debug.Log ("AAAAAAAAAAA");
		//Debug.Log (originalPosition);
		//Debug.Log (position);
		float dist = Vector2.Distance(originalPosition, position);
		if (dist > 0.1) {
			Vector2 opositeForce = (originalPosition - position);
			rigidbody2D.velocity += opositeForce*returnVelocity*0.02f;
		}   
		else {
			rigidbody2D.velocity = rigidbody2D.velocity * 0.6f;
		}
		
		rigidbody2D.velocity = rigidbody2D.velocity * 0.95f;
	}

}
