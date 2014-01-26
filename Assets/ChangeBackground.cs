using UnityEngine;
using System.Collections;

public class ChangeBackground : MonoBehaviour
{

	public Material dayMaterial, nightMaterial;
	private MeshRenderer renderer;

	void Start () {
		GameObject.Find ("EventManager").GetComponent<EventManager>().addListener(PlayerEvents.onPlayerStateChanged, gameObject);
		renderer = GetComponent<MeshRenderer> ();
	}

	public void onPlayerStateChanged(PlayerState state)
	{
		if (state == PlayerState.PLAYER_STATE_DEFAULT) {
			renderer.material = dayMaterial;
		} else {
			renderer.material = nightMaterial;
		}
	}


}
