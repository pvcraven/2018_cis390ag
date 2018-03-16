using System;
using System.Collections.Generic;

[System.Serializable]
public class DialogEvent {
	public double x_position; // The x coordinate the player would pass to trigger the event
	public List<string> messages; // A message tied to the event
	public int current_message; // The current message being displayed in the conversation

	public DialogEvent () {
		messages = new List<string> ();
		current_message = 0;
	}
}
