using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
	#region Fields
	private bool hasSavedGame;
	
	public Transform mainMenu;
	public Transform keyBindingsMenu;

	public static readonly String TRAINING_LEVEL_NAME = "Training Level";
	public static readonly String MAIN_MENU_NAME = "Main Menu";
	public static readonly String LEVEL_1_NAME = "Level 1";
	public static readonly String LEVEL_2_NAME = "Level2New";
	#endregion
	
	public void ResumeGame()
	{
		if (hasSavedGame)
		{
			//TODO: Add code to resume a game once the saving/loading functionality is implemented 
		}
		else
		{
			StartNewGame();
		}
	}
	
	public void StartNewGame()
	{
		SceneManager.LoadScene(TRAINING_LEVEL_NAME);
	}

	public bool HasSavedGame
	{
		get { return hasSavedGame; }
		set { hasSavedGame = value; }
	}
	
	public void OpenKeyBindingsMenu(bool isOpen)
	{
		if (isOpen)
		{
			keyBindingsMenu.gameObject.SetActive(true);
			mainMenu.gameObject.SetActive(false);
		}
		else
		{
			keyBindingsMenu.gameObject.SetActive(false);
			mainMenu.gameObject.SetActive(true);
		}
	}
	
	public void ExitProgram()
	{
		// Closes Unity game if it's a full application
		Application.Quit();
	}
	
	public void LoadLevel1()
	{
		SceneManager.LoadScene(LEVEL_1_NAME);
	}
	
	public void LoadLevel2()
	{
		SceneManager.LoadScene(LEVEL_2_NAME);
	}
}
