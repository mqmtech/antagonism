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

		setActiveChilds ();
	}

	// Update is called once per frame
	void Update () {

		Vector2 dir = transform.position - player.position;

		float dist = dir.magnitude;

		if (dist < radius) {
			applyOnEnter(player);
		}

		setActiveChilds ();
	}

	public void switchMode()
	{
		if (mode == NPCMode.MODE_FRIEND)
		{
			mode = NPCMode.MODE_ENEMY;
		} else {
			mode = NPCMode.MODE_FRIEND;
		}

		setActiveChilds();
	}

	void OnTriggerEnter(Collider other)
	{
		applyOnEnter (other.transform);
	}

	void applyOnEnter(Transform other)
	{
		if(isActivated) {
			return;
		}
		isActivated = true;

		if (mode == NPCMode.MODE_FRIEND)
		{
			Debug.Log("Player gets double jumps!!");
			other.GetComponent<PlayerControl>().setDoubleJumps(maxDoubleJumps);

			Instantiate(particlesOnEnemy, transform.position, Quaternion.identity);

			// Destroy
			Destroy(gameObject);
		} else {
			Debug.Log("Players has died!!");

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
