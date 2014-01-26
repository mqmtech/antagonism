using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour {

	
	public float scrollSpeed = 6.2f;
	public float distanceBetweenPlatforms = 4;
	public float distanceBetweenItems = 10;
	public List<GameObject> platformPrefabs;
	public List<GameObject> itemPrefabs;
	private float nextPlatformX = 0, nextPlatformY = 0;
	private float nextItemX = 0;
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
		position.x += scrollSpeed*Time.deltaTime;
		scrollSpeed += 0.05f*Time.deltaTime;
		this.transform.position = position;

		//Create new platforms
		if (position.x > nextPlatformX) {
			nextPlatformX = position.x + 2*distanceBetweenPlatforms;
			Vector3 rightBorder = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1,0.5f,-position.z));
			rightBorder.x += 5;
			nextPlatformY = nextPlatformY + Random.Range(-5,5);
			nextPlatformY = Mathf.Clamp(nextPlatformY, minY, maxY);
			rightBorder.y = nextPlatformY;

			int index = Random.Range(0, platformPrefabs.Count);
			GameObject.Instantiate( platformPrefabs[index], rightBorder, this.transform.rotation);
		}


		if (position.x > nextItemX && Random.value < 0.0045) {
			Vector3 rightBorder = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1,0.5f,-position.z));
			rightBorder.x += distanceBetweenItems;
			rightBorder.y = nextPlatformY + Random.Range(2,10);
			rightBorder.y = Mathf.Clamp(rightBorder.y, minY, maxY);
			int index = Random.Range(0, itemPrefabs.Count);
			GameObject.Instantiate( itemPrefabs[index], rightBorder, this.transform.rotation);

			nextItemX = position.x + distanceBetweenItems;
		}
	}


	void OnGUI() {
		GUI.skin.label.fontSize  = 24;
		GUI.Label (new Rect (10, 10, 200, 50), "Distance: " + Mathf.Floor(this.transform.position.x));

	}
}




