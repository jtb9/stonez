using System;
using UnityEngine;

public class TreeInteractible : MonoBehaviour
{
    public MeshRenderer treeMaterial;
    public int resourceCount = 10;
    public int maxResourceCount = 10;
    public Interactible i;

    public Color normalColor;
    public Color disabledColor;

    void Start()
    {
        i = GetComponent<Interactible>();
    }

    public InventoryItem itemType
    {
        get
        {
            if (UnityEngine.Random.Range(0, 10) >= 8) {
                return new InventoryItem()
                {
                    itemName = "sap",
                    itemQuantity = 1
                };
            }
            else {
                int quantityCalculation = 1 * Global._woodcutLevel * (int)Math.Round((i.DistanceToHome() / 50.0f));

                // minimum of one log per drop
                if (quantityCalculation <= 0) {
                    quantityCalculation = 1;
                }

                return new InventoryItem()
                {
                    itemName = "wood",
                    itemQuantity = quantityCalculation
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
