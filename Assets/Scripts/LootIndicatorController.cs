using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class LootIndicatorController : MonoBehaviour
{
    private VisualElement rootElement;

    public float floatSpeed = 50f;
    public float lifetime = 2f;

    void Start()
    {
        var uiDocument = GameObject.Find("Overlay").GetComponent<UIDocument>();
        rootElement = uiDocument.rootVisualElement;
    }

    public void SpawnLootIcon(Texture2D iconTexture, int quantity)
    {
        Debug.Log("spawning icon");

        // Create the icon element
        VisualElement icon = new VisualElement();
        icon.style.width = 50;
        icon.style.height = 50;
        icon.style.position = Position.Absolute;
        icon.style.backgroundImage = new StyleBackground(iconTexture);
        icon.style.left = 65;
        icon.style.bottom = 65;

         Label quantityLabel = new Label(quantity.ToString());
        quantityLabel.style.fontSize = 16;
        quantityLabel.style.color = new StyleColor(Color.white);
        quantityLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        quantityLabel.style.width = icon.resolvedStyle.width;  // Match the width of the icon
        quantityLabel.style.height = icon.resolvedStyle.height; // Match the height of the icon
        quantityLabel.style.position = Position.Relative;
        quantityLabel.style.top = -20;
        quantityLabel.style.left = -20;
        quantityLabel.style.borderTopLeftRadius = 6;
        quantityLabel.style.borderBottomLeftRadius = 6;
        quantityLabel.style.borderTopRightRadius = 6;
        quantityLabel.style.borderBottomRightRadius = 6;
        quantityLabel.style.backgroundColor = new StyleColor(Color.black);

        // Add the quantity label to the icon
        icon.Add(quantityLabel);

        // Add the icon to the root element
        rootElement.Add(icon);

        // Start the float and fade effect
        StartCoroutine(FloatAndFade(icon, quantityLabel));
    }

 private IEnumerator FloatAndFade(VisualElement icon, Label quantityLabel)
    {
        float elapsedTime = 0;
        float startY = icon.resolvedStyle.bottom;

        while (elapsedTime < lifetime)
        {
            // Update position
            icon.style.bottom = startY + elapsedTime * floatSpeed;
            //quantityLabel.style.bottom = startY + elapsedTime * floatSpeed; // Update label position to match icon

            // Update opacity
            float alpha = 1 - (elapsedTime / lifetime);
            icon.style.opacity = alpha;
            quantityLabel.style.opacity = alpha; // Fade the label along with the icon

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Remove the icon after the effect
        icon.RemoveFromHierarchy();
    }
}
