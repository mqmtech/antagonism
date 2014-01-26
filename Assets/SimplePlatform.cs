using UnityEngine;
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

		if(null == other.rigidbody) {
			return;
		}

		Vector3 vel = other.rigidbody.velocity;
		transform.parent = null;
		//Debug.Log(gameObject.name + " we're hit!");

		BroadcastMessage("onMotionActivate", new Vector2(vel.x*0.05f, vel.y*0.05f), SendMessageOptions.DontRequireReceiver);

		if (null != rigidbody) {
			rigidbody.isKinematic = false;
			collider.isTrigger = false;
			rigidbody.velocity = vel * 0.001f;
		}
	}
}
