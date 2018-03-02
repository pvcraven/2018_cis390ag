using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;

public class SaveGame : MonoBehaviour
{
	public Transform player;

	void Awake()
	{
		player.position = new Vector2(
			PlayerPrefs.GetFloat("playerPosX"),
			PlayerPrefs.GetFloat("playerPosY"));
	}

	private void SaveGameSettings()
	{
		// Get player location
		PlayerPrefs.SetFloat("playerPosX", player.position.x);
		PlayerPrefs.SetFloat("playerPosY", (float)(player.position.y + .4));
	}

	public void ExitToMainMenu()
	{
		SaveGameSettings();
		Time.timeScale = 1;
		SceneManager.LoadScene("Main Menu");
	}

	public void ExitProgram()
	{
		SaveGameSettings();
		
		// Closes Unity game in editor
		EditorApplication.isPlaying = false;
		
		// Closes Unity game if it's a full application
		Application.Quit();
	}
}
