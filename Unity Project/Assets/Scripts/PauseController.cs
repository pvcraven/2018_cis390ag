using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    private bool gamePaused = false;

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (!gamePaused)
            {
                PauseGame();
            }
            else if (gamePaused)
            {
                UnPauseGame();
            }
        }
    }

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
}
