using UnityEngine;
using System.Collections;

public class Action : MonoBehaviour {

	Vector2 originalPosition;
	float returnVelocity = 10;

	void Start() {
		originalPosition = new Vector2(transform.position.x, transform.position.y);
	}


	// Use this for initialization
	void onMotionActivate (Vector2 velocity) {
		rigidbody2D.velocity += velocity * 10;
		Debug.Log (velocity);
	}
	
	void Update () {
		Vector2 position = new Vector2(transform.position.x, transform.position.y);
		//Debug.Log ("AAAAAAAAAAA");
		//Debug.Log (originalPosition);
		//Debug.Log (position);
		float dist = Vector2.Distance(originalPosition, position);
		if (dist > 0.1) {
			Vector2 opositeForce = (originalPosition - position);
			rigidbody2D.velocity += opositeForce*returnVelocity;
			Debug.Log (rigidbody2D.velocity);

		}   
		else {
			rigidbody2D.velocity = rigidbody2D.velocity * 5 *Time.deltaTime;
		}
	}

}
