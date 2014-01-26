using UnityEngine;
using System.Collections;

public class RestartOnOutOfScreen : MonoBehaviour {
		
	bool dead = false;

	void Update () {
		Vector3 screenEdge = Camera.main.ViewportToWorldPoint(new Vector3(0,0,1));
		if (transform.position.y < screenEdge.y - 7f || transform.position.x < screenEdge.x - 2f) {
			GetComponent<PlayerControl>().enabled = false;
			Camera.main.GetComponent<CameraScript>().enabled = false;
			dead = true;
		}
		
		if (dead && (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown ("Fire1"))) {

			Application.LoadLevel(Application.loadedLevel);
		}


	}

	void OnGUI () {
		if (dead) {
			GUI.skin.label.fontSize  = 38;
			GUI.Label (new Rect (50, 50, 300, 50), "Distance: " + Mathf.Floor(Camera.main.transform.position.x));
		}
	}
}
