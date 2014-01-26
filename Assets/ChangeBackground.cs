using UnityEngine;
using System.Collections;

public class ChangeBackground : MonoBehaviour
{

	private Material material;
	private float lerp = 0;
	private bool positive = false; 

	void Start () {
		GameObject.Find ("EventManager").GetComponent<EventManager>().addListener(PlayerEvents.onPlayerStateChanged, gameObject);
		material = renderer.material;
	}

	void Update() {
		if (positive) {
			lerp += Time.deltaTime;
		} else {
			lerp -= Time.deltaTime;
		}
		lerp = Mathf.Clamp (lerp, 0, 1);
		//Debug.Log (lerp);
		material.SetFloat ("_Blend", lerp);
	}

	public void onPlayerStateChanged(PlayerState state)
	{
		if (state == PlayerState.PLAYER_STATE_DEFAULT) {
			positive = false;
		} else {
			positive = true;
		}
	}


}
