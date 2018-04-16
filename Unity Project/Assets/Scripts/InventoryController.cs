using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject PausedControlObject;
    public AudioClip[] audioclips;

    private bool inventoryIsOpen = false;
    private PauseController pauseGame;
    private PlayerController player;

    private const int itemSlotsNum = 12;
    private GameObject[] inventoryItems = new GameObject[itemSlotsNum];
    private GameObject[] inventorySlots;
    private int numOfWeapons = 0;

    private AudioSource audiosource;

    void Start()
    {
        pauseGame = PausedControlObject.GetComponent<PauseController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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

    public bool WeaponIsInInventory()
    {
       foreach (var item in inventoryItems)
        {
            if (item != null && item.tag == "Weapon")
                return true;
        }

        return false;
    }

    public bool AbleToSwitchWeapons()
    {
        if (numOfWeapons >= 2)
            return true;

        return false;
    }

    public void InventoryItemClick()
    {
        var itemClicked = EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < itemSlotsNum; i++)
        {
            if ((inventorySlots[i] == itemClicked) && (inventoryItems[i] != null))
            {
                if (inventoryItems[i].tag == "Water")
                {
                    player.tory.ConsumeEdibleItem();
                    audiosource.clip = audioclips[2];
                    audiosource.Play();
                    RemoveItem(i);
                }
                else if (inventoryItems[i].tag == "Food")
                {
                    player.tory.ConsumeEdibleItem();
                    audiosource.clip = audioclips[3];
                    audiosource.Play();
                    RemoveItem(i);
                }
                else if (inventoryItems[i].tag == "Weapon")
                {
                    // do nothing
                }
                else if (inventoryItems[i].tag == "HealthPack")
                {
                    player.tory.UseHealthPack();
                    Debug.Log("Health Pack Increased health to: " + player.tory.Health);
                    RemoveItem(i);
                }

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

                if (pickupItem.tag == "Weapon")
                    numOfWeapons++;

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