using UnityEngine;
using System.Collections;

public class LimitVertical : MonoBehaviour {

	float value;

	// Use this for initialization
	void Start () {
		value = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.x = value;
		transform.position = pos;
	}
}
