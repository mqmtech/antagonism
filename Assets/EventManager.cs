using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

	Dictionary<string,List<GameObject>> gameObjectMap = new Dictionary<string, List<GameObject>>();

	public void addListener(string eventName, GameObject go)
	{
		List<GameObject> listeners = null;

		if (!gameObjectMap.ContainsKey (eventName)) {
			listeners = new List<GameObject> ();
			gameObjectMap.Add(eventName, listeners);
		} else {
			gameObjectMap.TryGetValue(eventName, out listeners);
		}

		listeners.Add (go);
	}


	public void BroadcastEvent(string eventName, object parameter)
	{
		List<GameObject> listeners = null;
		gameObjectMap.TryGetValue(eventName, out listeners);

		if (null != listeners) {
			foreach (GameObject go in listeners) {
				Debug.Log ("Broadcasting Event to go: " + go.name + ", eventName is: " + eventName);
				go.BroadcastMessage(eventName, parameter);
			}
		}
	}
}
