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

    void UpdategUI() {
        SetLabel("gold", "Current Gold: " + Global._gold.ToString());
        SetLabel("stats", Global.statsAsString());
        SetLabel("inventory", inventory.toString());
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
}
