using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ConfirmationHandler : MonoBehaviour
{
    public static bool showing = false;
    public static String message = "";
    public static bool done = false;
    public static bool confirmed = false;

    public static void Show(String message) {
        ConfirmationHandler.done = false;
        ConfirmationHandler.confirmed = false;
        ConfirmationHandler.message = message;
        ConfirmationHandler.showing = true;

        
    }

    public static void Clear() {
        ConfirmationHandler.done = false;
        ConfirmationHandler.confirmed = false;
        ConfirmationHandler.message = "";
        ConfirmationHandler.showing = false;
    }

    private UIDocument uIDocument;
    void Start()
    {
        uIDocument = GetComponent<UIDocument>();
        uIDocument.enabled = false;
    }

    bool hasListener = false;
    void Update()
    {
        if (ConfirmationHandler.showing) {
            uIDocument.enabled = ConfirmationHandler.showing;
            Label q = uIDocument.rootVisualElement.Query<Label>("question");
            q.text = ConfirmationHandler.message;

            Button yes = uIDocument.rootVisualElement.Query<Button>("yes");
            Button no = uIDocument.rootVisualElement.Query<Button>("no");

            if (hasListener == false) {
                hasListener = true;
                yes.clicked += onYes;
                no.clicked += onNo;
            }
        }
        else {
            uIDocument.enabled = false;
            hasListener = false;
        }
    }

    public void onYes() {
        ConfirmationHandler.done = true;
        ConfirmationHandler.confirmed = true;
        ConfirmationHandler.showing = false;
    }

    public void onNo() {
        ConfirmationHandler.done = true;
        ConfirmationHandler.confirmed = false;
        ConfirmationHandler.showing = false;
    }
}
