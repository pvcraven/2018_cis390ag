using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    private bool gamePaused = false;

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
