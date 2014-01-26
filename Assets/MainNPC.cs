using UnityEngine;
using System.Collections;

public enum NPCMode
{
	MODE_ENEMY = 0,
	MODE_FRIEND
}
public class MainNPC : MonoBehaviour
{
	public NPCMode mode = NPCMode.MODE_FRIEND;

	GameObject friendPart;
	GameObject enemyPart;

	bool isActivated = false;

	public int maxDoubleJumps = 4;

	public float force = 3f;

	public float radius = 3;

	Transform player;

	public Transform particlesOnEnemy;
	public Transform particlesOnFriend;

	void Awake()
	{
		friendPart = transform.FindChild ("Friend").gameObject;
		enemyPart = transform.FindChild ("Enemy").gameObject;

		player = GameObject.FindGameObjectWithTag ("Player").transform;

		int r = Random.Range (0, 2);

		if (r < 1) {
			mode = NPCMode.MODE_ENEMY;
		} else {
			mode = NPCMode.MODE_FRIEND;
		}

		GameObject.Find ("EventManager").GetComponent<EventManager>().addListener(PlayerEvents.onPlayerStateChanged, gameObject);

		setActiveChilds ();
	}

	void onPlayerStateChanged()
	{
		switchMode ();
	}

	// Update is called once per frame
	void Update () 
	{
		//Debug.Log ("NPC position: " + transform.position);

		Vector2 dir = transform.position - player.position;

		float dist = dir.magnitude;

		if (dist < radius) {
			applyOnEnter(player);
		}

		setActiveChilds ();
	}

	void randomMove()
	{
		float rX = Random.Range (-0.2f, 0.2f);
		float rY = Random.Range (-0.2f, 0.2f);

		rigidbody2D.velocity += new Vector2 (rX, rY);
	}

	public void switchMode()
	{
		if (mode == NPCMode.MODE_FRIEND)
		{
			mode = NPCMode.MODE_ENEMY;
		} 
		else {
			mode = NPCMode.MODE_FRIEND;
		}

		setActiveChilds();
	}

	void OnTriggerEnter(Collider other)
	{
	}

	void applyOnEnter(Transform other)
	{
		if(isActivated) {
			return;
		}
		isActivated = true;

		if (mode == NPCMode.MODE_FRIEND)
		{
			other.GetComponent<PlayerControl>().setDoubleJumps(maxDoubleJumps);

			Instantiate(particlesOnEnemy, transform.position, Quaternion.identity);

			// Destroy
			Destroy(gameObject);
		} else {
			Application.LoadLevel(Application.loadedLevel);
			Instantiate(particlesOnFriend, transform.position, Quaternion.identity);
		}
	}

	void setActiveChilds()
	{
		if (mode == NPCMode.MODE_FRIEND) {
			friendPart.SetActive (true);
			enemyPart.SetActive (false);
		} else {
			friendPart.SetActive (false);
			enemyPart.SetActive (true);
		}
	}
}
