using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SplashScreenController : MonoBehaviour
{
    private UIDocument uiDocument;
    private Inventory inventory;

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        uiDocument.enabled = !Global.temp_hasStarted;

        // Button b1 = uiDocument.rootVisualElement.Query<Button>("newgame");
        // b1.clicked += NewGameClicked;

        Button b2 = uiDocument.rootVisualElement.Query<Button>("continue");
        b2.clicked += ContinueClicked;
    }

    void NewGameClicked() {
        Global.ResetStats();
        uiDocument.enabled = false;
        Global.temp_hasStarted = true;
    }

    void ContinueClicked() {
        uiDocument.enabled = false;
        Global.temp_hasStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
