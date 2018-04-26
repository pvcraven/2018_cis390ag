using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertSystem : MonoBehaviour
{
	public Text textBox;
	public List<GameEvent> events;
	private List<bool> eventsHappened;

	// Use this for initialization
	void Start () {
		eventsHappened = new List<bool>();
		for(int i = 0; i < events.Count; i++)
		{
			eventsHappened.Add(false);
		}
		textBox.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		checkForEvents();
	}

	void checkForEvents()
	{
		for(int i = 0; i < events.Count; i++)
		{
			//Debug.Log(transform.position.x + " " + events[i].x_position + " " + eventsHappened[i]);
			if (events[i].x_position <= transform.position.x && !eventsHappened[i])
			{
				eventsHappened[i] = true;
				textBox.text = events[i].message;
				textBox.enabled = true;
			}
		}
	}
}
