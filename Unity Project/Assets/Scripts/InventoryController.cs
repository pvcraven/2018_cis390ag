using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject PausedControlObject;

    private bool inventoryIsOpen = false;
    private PauseController pauseGame;

    private const int itemSlotsNum = 3;
    private GameObject[] inventoryItems = new GameObject[itemSlotsNum];
    private GameObject[] inventorySlots;

    void Start()
    {
        pauseGame = PausedControlObject.GetComponent<PauseController>();
        inventorySlots = GameObject.FindGameObjectsWithTag("InventorySlot");
        inventoryPanel.SetActive(false);
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

    public void InventoryItemClick()
    {
        var itemClicked = EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < itemSlotsNum; i++)
        {
            if ((inventorySlots[i] == itemClicked) && (inventoryItems[i] != null))
            {
                RemoveItem(i);
                return;
            }
        }
    }

    public bool AddItem(GameObject pickupItem)
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] == null)
            {
                var newPickupItem = Instantiate(pickupItem);
                newPickupItem.transform.parent = inventoryPanel.transform;
                inventoryItems[i] = newPickupItem;

                var tempSprite = pickupItem.GetComponent<SpriteRenderer>().sprite;
                inventorySlots[i].GetComponent<Image>().sprite = tempSprite;

                return true;
            }
        }
        return false;
    }

    private void RemoveItem(int position)
    {
        inventoryItems[position] = null;
        inventorySlots[position].GetComponent<Image>().sprite = null;
    }
}