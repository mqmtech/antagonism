using UnityEngine;
using System.Collections;

public class RestartOnOutOfScreen : MonoBehaviour {
		
	// Update is called once per frame
	void Update () {
		Vector3 screenEdge = Camera.main.ViewportToWorldPoint(new Vector3(0,0,1));
		if (transform.position.y < screenEdge.y - 5f || transform.position.x < screenEdge.x - 2f) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
