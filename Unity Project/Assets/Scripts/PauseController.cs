using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour
{
	public Transform pauseMenu;
	public Transform defaultPauseMenu;
	public Transform settingsMenu;
	public Transform keyBindingsMenu;
	public Transform player;

	private bool gamePaused = false;

	// Update is called once per frame
	void Update()
	{
		GetInput();
	}

	private void GetInput()
	{
		if (Input.GetButtonDown("Pause"))
		{
			if (!gamePaused)
			{
				ShowPauseMenu();
			}
			else
			{
				ResumeGame();
			}
		}
	}

	#region Pausing
	public void PauseGame()
	{
		Time.timeScale = 0;
		TogglePaused();
	}

	public void UnPauseGame()
	{
		Time.timeScale = 1;
		TogglePaused();
	}

	private void TogglePaused()
	{
		gamePaused = !gamePaused;
	}
	
	private void ShowPauseMenu()
	{
		pauseMenu.gameObject.SetActive(true);
		defaultPauseMenu.gameObject.SetActive(true);
		HideSubMenus();
		PauseGame();
	}

	public void ResumeGame()
	{
		pauseMenu.gameObject.SetActive(false);
		HideSubMenus();
		UnPauseGame();
	}
	#endregion

	#region Navigate Menus
	public void OpenSettingsMenu(bool isOpen)
	{
		if (isOpen)
		{
			settingsMenu.gameObject.SetActive(true);
			defaultPauseMenu.gameObject.SetActive(false);
		}
		else
		{
			settingsMenu.gameObject.SetActive(false);
			defaultPauseMenu.gameObject.SetActive(true);
		}
	}
	
	public void OpenKeyBindingsMenu(bool isOpen)
	{
		if (isOpen)
		{
			keyBindingsMenu.gameObject.SetActive(true);
			defaultPauseMenu.gameObject.SetActive(false);
		}
		else
		{
			keyBindingsMenu.gameObject.SetActive(false);
			defaultPauseMenu.gameObject.SetActive(true);
		}
	}
	
	private void HideSubMenus()
	{
		settingsMenu.gameObject.SetActive(false);
		keyBindingsMenu.gameObject.SetActive(false);
	}
	#endregion
}
