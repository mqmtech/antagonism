using UnityEngine;
using System.Collections;

public class LeapFingerColliderController : MonoBehaviour {

	Transform target = null;

	Vector3 offset;
	public float zPosistion = 0f;

	LeapControllerOffset controllerOffset = null;

	void Awake()
	{
		target = GameObject.Find ("Main Camera").transform;

		controllerOffset = GameObject.Find ("LeapControllerOffset").GetComponent<LeapControllerOffset> ();
		offset = controllerOffset.offset;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		repos ();
	}

	void FixedUpdate()
	{
		repos ();
	}

	void OnColliderEnter(Collider other)
	{
		Debug.Log ("the other is: " + other.name);
	}

	void repos()
	{
		Transform parentTrans = transform.parent;
		
		if (parentTrans == null) {
			return;
		}

		offset = controllerOffset.offset;
		transform.position = new Vector3 (parentTrans.position.x + offset.x,
		                                  parentTrans.position.y + offset.y,
		                                  0f);
	}
}
