using UnityEngine;
using System.Collections;

public class destroyFarElements : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, 15f);
	}

}
