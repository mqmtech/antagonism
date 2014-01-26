using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

	void Update () {
		

		if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown ("Fire1")) {
			
			Application.LoadLevel(Application.loadedLevel+1);
			
		}
	}
}
