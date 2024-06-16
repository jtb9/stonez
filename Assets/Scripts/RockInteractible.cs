using System;
using UnityEngine;

public class RockInteractible : MonoBehaviour
{
 public MeshRenderer treeMaterial;
    public int resourceCount = 10;
    public int maxResourceCount = 10;

    public Color normalColor;
    public Color disabledColor;

    void Start()
    {

    }

    public InventoryItem itemType
    {
        get
        {
            if (UnityEngine.Random.Range(0, 10) >= 8) {
                return new InventoryItem()
                {
                    itemName = "gem",
                    itemQuantity = 2
                };
            }
            else {
                return new InventoryItem()
                {
                    itemName = "rock",
                    itemQuantity = 1 * Global._miningLevel
                };
            }
        }
    }

    DateTime lastSpawn = DateTime.Now;
    void Update()
    {
        if ((DateTime.Now - lastSpawn).TotalSeconds >= 10)
        {
            if (resourceCount < maxResourceCount)
            {
                resourceCount += 1;
                lastSpawn = DateTime.Now;
            }
        }


        if (canGetResource)
        {
            // show the normal tree
            treeMaterial.material.color = normalColor;
        }
        else
        {
            // show a depleted tree
            treeMaterial.material.color = disabledColor;
        }
    }

    public void TakeResource()
    {
        resourceCount -= 1;
    }

    public bool canGetResource
    {
        get
        {
            if (resourceCount >= 1)
            {
                return true;
            }

            return false;
        }
    }
}
