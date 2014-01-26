using UnityEngine;
using System.Collections;

public class PostProcessController : MonoBehaviour
{
	SepiaToneEffect sepiaTone;
	MotionBlur motionBlur;

	Transform player;

	// Use this for initialization
	void Start () {
		sepiaTone = GetComponent<SepiaToneEffect> ();
		motionBlur = GetComponent<MotionBlur> ();

		player = GameObject.FindGameObjectWithTag ("Player").transform;

		GameObject.Find ("EventManager").GetComponent<EventManager>().addListener(PlayerEvents.onPlayerStateChanged, gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onPlayerStateChanged(PlayerState state)
	{
		enableEnemyState();

		Debug.Log ("running onPlayerStateChanged");
		if (state == PlayerState.PLAYER_STATE_DEFAULT) {
			enableDefaultState ();
		} else {
			enableEnemyState();
		}
	}

	public void enableDefaultState()
	{
		sepiaTone.deactivate ();
		motionBlur.deactivate ();
	}

	public void enableEnemyState()
	{
		sepiaTone.activate ();
		motionBlur.activate ();
	}
}
