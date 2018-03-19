using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO; 

using System;

public class DialogManager : MonoBehaviour
{
	public GameObject dialog_box;
	public List<DialogEvent> list_of_dialog_events;
    public GameObject PausedControlObject;
    private PauseController pauseGame;


    private Rigidbody2D rb;
	private bool paused_for_dialog = false;
	private List<bool> dialog_that_has_taken_place;
	private int current_dialog_event = 0;



	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
        pauseGame = PausedControlObject.GetComponent<PauseController>();

        load_dialog_from_file("Assets/Dialog/testing_grounds.txt");
		initialize_dialog_events ();
		hide_dialog_on_start ();
	}

	void initialize_dialog_events () {
		dialog_that_has_taken_place = new List<bool> ();
		for (int i = 0; i < list_of_dialog_events.Count; i++)
			dialog_that_has_taken_place.Add (false);
	}

	void hide_dialog_on_start () {
		dialog_box.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		check_if_event_triggered();

		if (paused_for_dialog) {
			if (Input.GetKeyUp("space")) {
				DialogEvent current_dialog = list_of_dialog_events [current_dialog_event];
				if (current_dialog.current_message == current_dialog.messages.Count - 1)
					unpause_and_hide_dialog ();
				else {
					current_dialog.current_message++;
					Text dialog_text = dialog_box.GetComponentInChildren<Text> ();
					dialog_text.text = current_dialog.messages[current_dialog.current_message];
				}
			}
		}
	}

	void check_if_event_triggered()
	{
		for(int i = 0; i<list_of_dialog_events.Count; i++)
		{
			//Debug.Log(transform.position.x + " " + list_of_dialog_events[i].x_position + " " + dialog_that_has_taken_place[i]);
			if (list_of_dialog_events[i].x_position <= transform.position.x && !dialog_that_has_taken_place[i]) {
				current_dialog_event = i;
				dialog_that_has_taken_place[i] = true;
				Text dialog_text = dialog_box.GetComponentInChildren<Text> ();
				dialog_text.text = list_of_dialog_events[i].messages[list_of_dialog_events[i].current_message];
				dialog_box.SetActive (true);
				pause_until_space_is_pressed ();
			}
		}
	}

	void pause_until_space_is_pressed() {
        pauseGame.PauseGame();
        paused_for_dialog = true;
	}

	void unpause_and_hide_dialog() {
        pauseGame.UnPauseGame();
        paused_for_dialog = false;
		dialog_box.SetActive (false);
	}

	private bool load_dialog_from_file(string fileName) {
		try {
			string line;
			StreamReader theReader = new StreamReader(fileName, Encoding.Default);
			DialogEvent current_dialog_event = new DialogEvent();
			using (theReader) {
				do {
					line = theReader.ReadLine();

					if (line != null) {
						if(line.Contains("x-pos")) {
							string[] parts_of_line = line.Split(':');
							float x_position = float.Parse(parts_of_line[1]);
							current_dialog_event.x_position = x_position;
							Debug.Log("New Conversation at: "+x_position);
						} else if(line.Equals("") || line.Contains("***")) {
							list_of_dialog_events.Add(current_dialog_event);
							current_dialog_event = new DialogEvent();
							Debug.Log("Done Reading Event");
						} else {
							current_dialog_event.messages.Add(line);
							Debug.Log(line);
						}
						//Debug.Log(line);
					}
				} while (line != null);
				theReader.Close();
				return true;
			}
		}
		catch (Exception e) {
			Debug.Log(e.Message);
			return false;
		}
	}
}
