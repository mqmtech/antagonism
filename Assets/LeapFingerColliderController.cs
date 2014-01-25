using UnityEngine;
using System.Collections;

public class LeapFingerColliderController : MonoBehaviour {

	Transform target = null;

	Vector3 offset;

	void Awake()
	{
		target = GameObject.Find ("Main Camera").transform;

		offset = GameObject.Find ("LeapControllerOffset").transform.position;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//repos ();
	}

	void FixedUpdate()
	{
		repos ();
	}

	void repos()
	{
		Transform parentTrans = transform.parent;
		
		if (parentTrans == null) {
			return;
		}
		
		transform.position = new Vector3 (parentTrans.position.x + offset.x,
		                                  parentTrans.position.y + offset.y,
		                                  0);
	}
}
