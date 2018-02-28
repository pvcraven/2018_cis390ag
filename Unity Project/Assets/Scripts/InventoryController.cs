using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject PausedControlObject;

    private bool inventoryIsOpen = false;
    private PauseController pauseGame;

    void Start()
    {
        inventoryPanel.SetActive(false);
        pauseGame = PausedControlObject.GetComponent<PauseController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!inventoryIsOpen)
            {
                inventoryPanel.SetActive(true);
                inventoryIsOpen = true;
                pauseGame.PauseGame();
            }
            else
            {
                inventoryPanel.SetActive(false);
                inventoryIsOpen = false;
                pauseGame.UnPauseGame();
            }
        }
    }
}