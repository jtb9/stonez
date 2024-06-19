using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopMenuController : MonoBehaviour
{
    public static bool showing = false;

    public static void Open() {
        ShopMenuController.showing = true;
    }

    public static void Close() {
        ShopMenuController.showing = false;
    }
    
    private UIDocument uiDocument;
    private Inventory inventory;
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        uiDocument.enabled = false;
        inventory = GameObject.FindFirstObjectByType<Inventory>();
    }

    void Bind() {
        bindingReady = true;

        BindButton("button1", OnClick1);
        BindButton("button2", OnClick2);
        BindButton("button3", OnClick3);
        BindButton("button4", OnClick4);
        BindButton("button5", OnClick5);
    }

    void BindButton(String name, Action target) {
        Button b1 = uiDocument.rootVisualElement.Query<Button>(name);
        b1.clicked += target;
    }

    void SetLabel(String labelName, String value) {
        Label l = uiDocument.rootVisualElement.Query<Label>(labelName);
        l.text = value;
    }

    bool bindingReady = false;
    void Update()
    {
        uiDocument.enabled = ShopMenuController.showing;

        if (ShopMenuController.showing == false) {
            bindingReady = false;
        }
        else {
            if (bindingReady == false) {
                Bind();
            }
        }

        if (ShopMenuController.showing) {
            UpdategUI();
        }
    }

    String getQuantityByName(String name) {
        try {
            return inventory.getByName(name).itemQuantity.ToString();
        }
        catch(System.Exception e) {
            return "0";
        }
    }

    String getValueByName(String name) {
        try {
            return (inventory.getByName(name).itemQuantity * inventory.getValue(name)).ToString();
        }
        catch(System.Exception e) {
            return "0";
        }
    }

    void UpdategUI() {
        SetLabel("gold", Global._gold.ToString());
        SetLabel("attack", Global._attackLevel.ToString());
        SetLabel("strength", Global._strengthLevel.ToString());
        SetLabel("defense", Global._defenseLevel.ToString());
        SetLabel("skill", Global._nextChallengeLevel.ToString());
        SetLabel("woodcutting", Global._woodcutLevel.ToString());
        SetLabel("mining", Global._miningLevel.ToString());
        SetLabel("rocks", getQuantityByName("rock"));
        SetLabel("gems", getQuantityByName("gem"));
        SetLabel("sap", getQuantityByName("sap"));
        SetLabel("logs", getQuantityByName("wood"));

        SetLabel("rocks2", getValueByName("rock"));
        SetLabel("gems2", getValueByName("gem"));
        SetLabel("sap2", getValueByName("sap"));
        SetLabel("logs2", getValueByName("wood"));
    }

    void OnClick1() {
        
        for (int i = 0; i < inventory.items.Count; i++) {
            int targetSale = inventory.items[i].itemQuantity;
            int goldPerItem = inventory.getValue(inventory.items[i].itemName);
            
            if (inventory.removeItemByQuantity(inventory.items[i].itemName, targetSale)) {
                inventory.gold += goldPerItem * targetSale;
            }
        }
    }

    void OnClick2() {
        if (Global._gold >= 50) {
            Global._attackLevel += 1;
            inventory.gold -= 50;
        }
    }

    void OnClick3() {
        if (Global._gold >= 50) {
            Global._strengthLevel += 1;
            inventory.gold -= 50;
        }
    }

    void OnClick4() {
        ShopMenuController.Close();
    }

    void OnClick5() {
        if (Global._gold >= 50) {
            Global._defenseLevel += 1;
            inventory.gold -= 50;
        }
    }
}
