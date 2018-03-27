using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;

public class SaveGame : MonoBehaviour
{
	public Transform player;

	void Awake()
	{
		//loadPlayerPosition();
	}

	private void SaveGameSettings()
	{
		// Get player location
		PlayerPrefs.SetFloat("playerPosX", player.position.x);
		PlayerPrefs.SetFloat("playerPosY", (float)(player.position.y + 1));
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
	
	private void loadPlayerPosition()
	{
		this.player.position = new Vector2(
			PlayerPrefs.GetFloat("playerPosX"),
			PlayerPrefs.GetFloat("playerPosY"));
	}
}
