using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour {

	
	public float speed = 4.5f;
	public float distanceBetweenPlatforms = 4;
	public List<GameObject> prefabs;
	private float nextPlatformX = 0, nextPlatformY = 0;
	private float minY = 0, maxY = 100;
	public GameObject firewall;


	void Start () {
		//Set firewall position
		Vector3 leftBorder = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0,0.5f,1));
		firewall.transform.position = leftBorder;

		nextPlatformY = leftBorder.y - 5;

		//Read screen geometry (TODO: Update on resize) 
		maxY = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1,1,1)).y - 5;
		minY = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1,0,1)).y + 5;

	}
	
	void Update () {
		//Scroll camera
		Vector3 position = this.transform.position;
		position.x += speed*Time.deltaTime;
		speed += 0.01f*Time.deltaTime;
		this.transform.position = position;

		//Create new platforms
		if (position.x > nextPlatformX) {
			nextPlatformX = position.x + 2*distanceBetweenPlatforms;
			Vector3 rightBorder = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1,0.5f,-position.z));
			rightBorder.x += distanceBetweenPlatforms;
			nextPlatformY = nextPlatformY + Random.Range(-5,5);
			nextPlatformY = Mathf.Clamp(nextPlatformY, minY, maxY);
			rightBorder.y = nextPlatformY;

			int index = Random.Range(0, prefabs.Count-1);

			GameObject.Instantiate( prefabs[index], rightBorder, this.transform.rotation);
		}

	}

}




