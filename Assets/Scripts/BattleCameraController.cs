using UnityEngine;

public class BattleCameraController : MonoBehaviour
{
// The target the camera will follow
    public Transform target;

    // The speed at which the camera will rotate to look at the target
    public float rotationSmoothSpeed = 0.125f;

    // The offset from the target position
    public Vector3 offset;

    void LateUpdate()
    {
        // Ensure the target is assigned
        if (target == null)
        {
            Debug.LogWarning("Target not assigned to CameraFollow script.");
            return;
        }

        // Define the desired position based on the target's position and offset
        Vector3 desiredPosition = target.position + offset;

        // Interpolate between the camera's current rotation and the desired rotation
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        Quaternion smoothedRotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotationSmoothSpeed);

        // Update the camera's rotation
        transform.rotation = smoothedRotation;
    }
}
