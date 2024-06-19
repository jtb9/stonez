using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BattleMenuController : MonoBehaviour
{
    public static bool showing = false;

    public static void Open() {
        BattleMenuController.showing = true;
    }

    public static void Close() {
        BattleMenuController.showing = false;
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
        BindButton("button3", OnClick4);
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
        uiDocument.enabled = BattleMenuController.showing;

        if (BattleMenuController.showing == false) {
            bindingReady = false;
        }
        else {
            if (bindingReady == false) {
                Bind();
            }
        }

        if (BattleMenuController.showing) {
            UpdategUI();
        }
    }

    void UpdategUI() {
        // SetLabel("gold", "Current Gold: " + Global._gold.ToString());
        SetLabel("battle", Global._nextChallengeLevel.ToString());
        // SetLabel("attack", "Current Attack Level: " + Global._attackLevel.ToString());
        // SetLabel("strength", "Current Strength Level: " + Global._strengthLevel.ToString());
    }

    void OnClick1() {
        Global.temp_willChallenge = 0;
        Global.Save();
        BattleMenuController.Close();
        SceneManager.LoadScene("Battle");
    }

    void OnClick2() {
        Global.temp_willChallenge = 1;
        Global.Save();
        BattleMenuController.Close();
        SceneManager.LoadScene("Battle");
    }

    void OnClick4() {
        BattleMenuController.Close();
    }
}
