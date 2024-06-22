using UnityEngine;

public class TowerDefenseCameraController : MonoBehaviour
{
    public Camera mainCamera;
    public Joystick cameraJoystick;

    // Speed of the camera movement
    public float cameraSpeed = 5f;

    // Limits for the camera movement
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Vector3 targetPosition;

    void Start()
    {
        // Initialize the target position to the camera's initial position
        targetPosition = mainCamera.transform.position;
    }

    void Update()
    {
        // Get the direction from the joystick
        Vector2 direction = cameraJoystick.Direction;

        // Convert joystick direction to camera movement
        Vector3 movement = new Vector3(direction.x, 0, direction.y);

        // Adjust movement based on the camera's angle
        Vector3 adjustedMovement = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * movement;

        // Calculate the target position based on joystick direction and speed
        targetPosition += adjustedMovement * cameraSpeed * Time.deltaTime;

        // Clamp the target position within the specified bounds
        targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
        targetPosition.z = Mathf.Clamp(targetPosition.z, minBounds.y, maxBounds.y);

        // Smoothly move the camera to the target position
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * cameraSpeed);
    }
}
