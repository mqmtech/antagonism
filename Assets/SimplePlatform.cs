﻿using UnityEngine;
using System.Collections;

public class SimplePlatform : MonoBehaviour {

	Action action;
	void Awake()
	{
		action = GetComponent<Action> ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider otherCollider)
	{
		if (otherCollider.tag != "FingerTip") return;

		Transform other = otherCollider.gameObject.transform;

		if (other.parent != null) {
			other = other.parent.transform;
		}

		Vector3 vel = other.rigidbody.velocity;
		transform.parent = null;
		Debug.Log("we're hit!");

		BroadcastMessage("onMotionActivate",new Vector2(vel.x*0.05f, vel.y*0.05f));

		LeapFinger finger = other.GetComponent<LeapFinger> ();
		if(null == finger) {
			Debug.Log("finger is null");
		}

		if (null == rigidbody) {
			return;
		}

		Vector3 middleVel = finger.getMiddleVelocity ();

		rigidbody.isKinematic = false;
		collider.isTrigger = false;
		rigidbody.velocity = vel * 0.001f;
	}
}
