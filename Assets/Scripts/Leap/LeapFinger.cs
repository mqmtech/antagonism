using UnityEngine;
using System.Collections;
using Leap;

public class LeapFinger : MonoBehaviour
{
	public Vector3 prevPosition;
	public Vector3 direction;

	// parent hand
	public Hand m_hand;

	void Awake()
	{
		prevPosition = new Vector3 (0, 0, 0);
	}

	void Update()
	{
		direction = transform.position - prevPosition;
	}

	public Vector3 getMiddleVelocity()
	{
		FingerList list = m_hand.Fingers;

		int count = 0;
		Vector v = new Vector (0f,0f,0f);
		for (int i = 0; i < list.Count; i++) {
			Finger finger = list[i];
			v = v+finger.TipVelocity;
			count +=1;
		}

		v.x/=count;
		v.y/=count;
		v.z/=count;

		Debug.Log ("middle vel: " + v);
		return new Vector3(v.x, v.y, v.z);
	}
}
