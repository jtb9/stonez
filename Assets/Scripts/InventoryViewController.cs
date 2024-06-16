using System;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryViewController : MonoBehaviour
{
    public static bool showing = false;

    public static void Open() {
        InventoryViewController.showing = true;
    }

    public static void Close() {
        InventoryViewController.showing = false;
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

        BindButton("done", OnClick1);
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
        uiDocument.enabled = InventoryViewController.showing;

        if (InventoryViewController.showing == false) {
            bindingReady = false;
        }
        else {
            if (bindingReady == false) {
                Bind();
            }
        }

        if (InventoryViewController.showing) {
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

    void UpdategUI() {
        SetLabel("rocks", getQuantityByName("rock"));
        SetLabel("gems", getQuantityByName("gem"));
        SetLabel("sap", getQuantityByName("sap"));
        SetLabel("logs", getQuantityByName("wood"));
    }

    void OnClick1() {
        InventoryViewController.Close();
    }
}
