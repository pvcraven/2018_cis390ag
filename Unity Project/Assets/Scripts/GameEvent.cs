using System;

[System.Serializable]
public class GameEvent {
	public double x_position; // The x coordinate the player would pass to trigger the event
	public string message; // A message tied to the event

	public GameEvent () {
	}
}

