using System;
using UnityEngine;
using UnityEngine.UIElements;
public class SkillMenuController : MonoBehaviour
{
    public static bool showing = false;

    public static void Open() {
        SkillMenuController.showing = true;
    }

    public static void Close() {
        SkillMenuController.showing = false;
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
        uiDocument.enabled = SkillMenuController.showing;

        if (SkillMenuController.showing == false) {
            bindingReady = false;
        }
        else {
            if (bindingReady == false) {
                Bind();
            }
        }

        if (SkillMenuController.showing) {
            UpdategUI();
        }
    }

    void UpdategUI() {
        SetLabel("attack", Global._attackLevel.ToString());
        SetLabel("strength", Global._strengthLevel.ToString());
        SetLabel("defense", Global._defenseLevel.ToString());
        SetLabel("skill", Global._nextChallengeLevel.ToString());
        SetLabel("woodcutting", Global._woodcutLevel.ToString());
        SetLabel("mining", Global._miningLevel.ToString());
    }

    void OnClick1() {
        SkillMenuController.Close();
    }
}
