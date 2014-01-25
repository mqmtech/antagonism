using UnityEngine;
using System.Collections;

public class RestartOnOutOfScreen : MonoBehaviour {

	private Vector3 screenEdge;

	void Start () {
		screenEdge = Camera.main.ViewportToWorldPoint(new Vector3(0,0,1));
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < screenEdge.y - 5f || transform.position.x < screenEdge.x - 2f) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
