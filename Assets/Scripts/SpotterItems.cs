using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BouncerTools;
using UnityEngine.UI;

public class SpotterItems : MonoBehaviour
{
    public GameObject Object;
    public int itemsInInventory;
    public int totalItems;
    public SpotterItem itemEnum;
    public GameObject ItemImage;
    public GameObject MenuManager;
    public MenuManager MenuScript;


    void Update()
    {
        if (MenuManager == null)
        {
            MenuManager = GameObject.Find("MenuManager");
            MenuScript = MenuManager.GetComponent<MenuManager>();
            itemsInInventory = GetItemCount();
            totalItems = GetItemCount();
        }
    }

    public int GetItemCount()
    {
        switch (itemEnum)
        {
            case SpotterItem.Target:
                return MenuScript.targetCount;
            case SpotterItem.BoostPad:
                return MenuScript.boostCount;
            case SpotterItem.BouncePad:
                return MenuScript.bouncePadCount;
            case SpotterItem.Teleporter:
                return MenuScript.teleportCount;
            case SpotterItem.PlayerStart:
                return 1;
            default:
                return 0;
        }
    }
}
