using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryItem
{
    public String itemName = "";
    public int itemQuantity = 0;
}

public class Inventory : MonoBehaviour
{
    public static Texture2D[] iconLibrary;
    public static Texture2D getIconByName(String itemName) {
        return iconLibrary[0];
    }
    public int getValue(String itemName) {
        if (itemName == "wood") {
            return 1;
        }

        if (itemName == "rock") {
            return 2;
        }

        if (itemName == "gem") {
            return 15;
        }

        if (itemName == "sap") {
            return 9;
        }


        // unkown
        return 0;
    }

    public List<InventoryItem> items = new List<InventoryItem>();
    public int gold
    {
        get
        {
            return Global._gold;
        }
        set
        {
            Global._gold = value;
            Global.Save();
        }
    }

    public int itemQuantity(String name)
    {
        int total = 0;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == name)
            {
                total += items[i].itemQuantity;
            }
        }

        return total;
    }

    public InventoryItem getByName(String name) {
        if (items.Count <= 0) {
            return null;
        }

        var match = items[0];

        for (int i = 0; i < items.Count; i++) {
            if (items[i].itemName == name) {
                match = items[i];
            }
        }

        return match;
    }

    public void addItemByQuantity(InventoryItem newItem)
    {
        bool match = false;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == newItem.itemName)
            {
                items[i].itemQuantity += newItem.itemQuantity;
                match = true;
            }
        }

        if (match == false)
        {
            items.Add(newItem);
        }

        Save();
    }

    public bool removeItemByQuantity(String type, int itemQuantity)
    {
        bool match = false;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == type)
            {
                if (items[i].itemQuantity >= itemQuantity)
                {
                    items[i].itemQuantity -= itemQuantity;
                    match = true;
                }

            }
        }

        Save();

        return match;
    }

    public String toString()
    {
        String buffer = "Gold: " + gold.ToString() + " - ";

        for (int i = 0; i < items.Count; i++)
        {
            buffer += items[i].itemName + " (" + items[i].itemQuantity.ToString() + "), ";
        }

        return buffer;
    }

    public void Save()
    {
        String inventoryAsString = "";

        for (int i = 0; i < items.Count; i++)
        {
            inventoryAsString += items[i].itemName + "-" + items[i].itemQuantity.ToString();

            if (i < items.Count)
            {
                inventoryAsString += ";";
            }
        }

        Global._inventory = inventoryAsString;
        Global.Save();
    }

    public void Load()
    {
        Global.Load();

        // parse the inventory string
        String[] se = Global._inventory.Split(";");

        for (int i = 0; i < se.Length; i++)
        {
            if (se[i].Length > 0)
            {
                String[] bits = se[i].Split("-");

                items.Add(new InventoryItem()
                {
                    itemName = bits[0],
                    itemQuantity = Int32.Parse(bits[1])
                });
            }
        }
    }
}
