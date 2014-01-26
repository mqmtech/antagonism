using UnityEngine;
using System.Collections;

public class DestroyOnTime : MonoBehaviour {

	public float time = 0.5f;

	void Start () {
		Destroy (this.gameObject, time);
	}

}
