using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class FloatingIconIndicator : MonoBehaviour
{
    public Texture2D iconTexture;
    private UIDocument uiDocument;
    private VisualElement icon;
    private CameraController controller;

    public Vector3 iconOffset = new Vector3(0, 1, 0);

    public VisualElement CreateIconFor()
    {
        VisualElement icon = new VisualElement();
        icon.style.backgroundImage = new StyleBackground(iconTexture);
        icon.style.width = 35;
        icon.style.height = 35;
        icon.style.visibility = Visibility.Hidden;
        icon.style.position = Position.Absolute;
        uiDocument.rootVisualElement.Add(icon);
        return icon;
    }

    void Start()
    {
        controller = GameObject.FindFirstObjectByType<CameraController>();

        GameObject rootO = GameObject.Find("Overlay");
        uiDocument = rootO.GetComponent<UIDocument>();

        icon = CreateIconFor();
    }

    public float CalculateScale()
    {
        // Calculate scale based on screen size relative to 1920x1080
        float referenceResolution = 1200 * 800; // Reference resolution (1920x1080)
        float currentResolution = Screen.width * Screen.height;
        return Mathf.Sqrt(currentResolution / referenceResolution);
    }

    public Vector2 CalculatePosition(Vector3 worldPosition, float scale)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        // Adjust position to scale
        screenPosition.x *= scale;
        screenPosition.y *= scale;
        return new Vector2(screenPosition.x, Screen.height - screenPosition.y);
    }

    void Update()
    {
        Vector3 worldPosition = transform.position + iconOffset;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        if (screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height)
        {
            icon.style.visibility = Visibility.Visible;
            icon.style.width = 50;
            icon.style.height = 50;


            // Set the icon's position in the UI Document
            //icon.style.left = new Length((screenPosition.x / Screen.width) * 1200, LengthUnit.Pixel);
            //icon.style.top = new Length((screenPosition.y / Screen.height) * 800, LengthUnit.Pixel);

            icon.style.left = screenPosition.x - 50;
            icon.style.top = Screen.height - screenPosition.y - 50;
        }
        else
        {
            icon.style.visibility = Visibility.Hidden;
        }
    }
}
