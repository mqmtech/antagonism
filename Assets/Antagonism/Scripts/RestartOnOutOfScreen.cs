using UnityEngine;
using System.Collections;

public class RestartOnOutOfScreen : MonoBehaviour {

	private float screenEdge;

	void Start () {
		screenEdge = Camera.main.ViewportToWorldPoint(new Vector3(1,0,1)).y - 5;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < screenEdge) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
