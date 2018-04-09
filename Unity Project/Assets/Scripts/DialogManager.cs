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


	private bool paused_for_dialog = false;
	private List<bool> dialog_that_has_taken_place;
	private int current_dialog_event = 0;



	// Use this for initialization
	void Start () {
        pauseGame = PausedControlObject.GetComponent<PauseController>();

        load_dialog_from_file("Assets/Dialogue/training_level.txt");
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
			if (Input.GetKeyUp("return")) {
				reset_dialog_text ();
				DialogEvent current_dialog = list_of_dialog_events [current_dialog_event];
				if (current_dialog.current_message == current_dialog.messages.Count - 1)
					unpause_and_hide_dialog ();
				else {
					current_dialog.current_message++;
					Text dialog_text = dialog_box.GetComponentInChildren<Text> ();
					String text_to_display = format_text (dialog_text, current_dialog.messages[current_dialog.current_message]);
					dialog_text.text = text_to_display;
				}
			}
		}
	}

	string format_text (Text dialog_text, string str) {
		string return_value = str;
		if (str.Contains ("*BOLD*")) {
			//Debug.Log ("Bold");
			dialog_text.fontStyle = FontStyle.Bold;
			return_value = return_value.Replace ("*BOLD*", "");
		}
		if (str.Contains ("*ITALIC*")) {
			dialog_text.fontStyle = FontStyle.Italic;
			return_value = return_value.Replace ("*ITALIC*", "");
		}
		if (str.Contains ("*BIG*")) {
			dialog_text.fontSize = 20;
			return_value = return_value.Replace ("*BIG*", "");
		}
		return return_value;
	}

	void reset_dialog_text () {
		Text dialog_text = dialog_box.GetComponentInChildren<Text> ();
		dialog_text.fontStyle = FontStyle.Normal;
		dialog_text.fontSize = 14;
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
                if (list_of_dialog_events[i].messages.Count == 0)
                {
                    Debug.Log("Error, dialog event " + i + " has no text to go with it. Check for a double blank line in training_level.txt");
                    return;
                }
                String message = list_of_dialog_events[i].messages[list_of_dialog_events[i].current_message];
				String text_to_display = format_text (dialog_text, message);
				dialog_text.text = text_to_display;
				
				dialog_box.SetActive (true);

                Rigidbody2D rb = GetComponent<Rigidbody2D>();

                Vector2 dialog_position = new Vector2 (rb.position.x, rb.position.y+1.2f);
				dialog_box.transform.position = dialog_position;
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
							//Debug.Log("New Conversation at: "+x_position);
						} else if(line.Equals("") || line.Contains("***")) {
							list_of_dialog_events.Add(current_dialog_event);
							current_dialog_event = new DialogEvent();
							//Debug.Log("Done Reading Event");
						} else {
							current_dialog_event.messages.Add(line);
							//Debug.Log(line);
						}
						//Debug.Log(line);
					}
				} while (line != null);
				theReader.Close();
				return true;
			}
		}
		catch (Exception e) {
			Debug.LogError(e.Message);
			return false;
		}
	}
}
