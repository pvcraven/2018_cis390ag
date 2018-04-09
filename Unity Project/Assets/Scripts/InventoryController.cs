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

    private const int itemSlotsNum = 12;
    private GameObject[] inventoryItems = new GameObject[itemSlotsNum];
    private GameObject[] inventorySlots;

    private AudioSource audiosource;
    public AudioClip[] audioclips;
    void Start()
    {
        pauseGame = PausedControlObject.GetComponent<PauseController>();
        inventorySlots = GameObject.FindGameObjectsWithTag("InventorySlot");
        inventoryPanel.SetActive(false);
        audiosource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!inventoryIsOpen)
            {
                audiosource.clip = audioclips[0];
                audiosource.Play();
                inventoryPanel.SetActive(true);
                inventoryIsOpen = true;
                pauseGame.PauseGame();
            }
            else
            {
                audiosource.clip = audioclips[1];
                audiosource.Play();
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
                var currentSlot = inventorySlots[i].GetComponent<Image>();
                var tempSprite = pickupItem.GetComponent<SpriteRenderer>().sprite;

                var newPickupItem = Instantiate(pickupItem);
                newPickupItem.transform.parent = inventoryPanel.transform;
                inventoryItems[i] = newPickupItem;

                currentSlot.sprite = tempSprite;
                currentSlot.color = Color.white;

                return true;
            }
        }
        return false;
    }

    private void RemoveItem(int position)
    {
        var currentSlot = inventorySlots[position].GetComponent<Image>();
        inventoryItems[position] = null;
        currentSlot.sprite = null;
        currentSlot.color = Color.black;
    }
}