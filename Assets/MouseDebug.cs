using UnityEngine;
using System.Collections;

public class MouseDebug : MonoBehaviour {

	private Vector3 lastMousePosition;
	private Vector3 mouseMotion;

	void OnStart() {
		lastMousePosition = Input.mousePosition;
	}

	void Update() {
		mouseMotion = Input.mousePosition - lastMousePosition; 
		lastMousePosition = Input.mousePosition;

	}
	void OnMouseOver() {
		if (Input.GetMouseButtonDown(0)) {
			BroadcastMessage("onMotionActivate", new Vector2(mouseMotion.x, mouseMotion.y));
		}
	}

}
