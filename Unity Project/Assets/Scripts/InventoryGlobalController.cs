using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGlobalController : MonoBehaviour
{
    public static InventoryGlobalController Instance;

    public const int itemSlotsNum = 12;
    public GameObject[] inventoryItems = new GameObject[itemSlotsNum];

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }
}
