using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertSystem : MonoBehaviour
{
    [System.Serializable]
    public class Event
    {
        public double alertPosition;
        public string alertMessage;
    }

    public Text displayMessage;
    public List<Event> events;
    private List<bool> eventsHappened;
    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        eventsHappened = new List<bool>();
        for(int i = 0; i < events.Count; i++)
        {
            eventsHappened.Add(false);
        }
        displayMessage.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        checkForEvents();
	}

    void checkForEvents()
    {
        for(int i = 0; i<events.Count; i++)
        {
            Debug.Log(transform.position.x + " " + events[i].alertPosition + " " + eventsHappened[i]);
            if (events[i].alertPosition <= transform.position.x && !eventsHappened[i])
            {
                eventsHappened[i] = true;
                displayMessage.text = events[i].alertMessage;
                displayMessage.enabled = true;
            }
        }
    }
}
