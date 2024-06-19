using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public UIDocument mainUIDocument;

    public Transform target;
    public float targetRotation = 90.0f;
    public float zoom = 1.0f;
    public float targetZoom = 1.0f;
    public Vector3 offset = Vector3.zero;

    void Start()
    {
        Button left = mainUIDocument.rootVisualElement.Query<Button>("left");
        Button right = mainUIDocument.rootVisualElement.Query<Button>("right");

        left.clicked += LeftMove;
        right.clicked += RightMove;
    }

    void LeftMove() {
        targetRotation -= 5.0f;
    }

    void RightMove() {
        targetRotation += 5.0f;
    }

    void Update()
    {
        float tempZoom = Math.Abs(3.0f - zoom);

        if (tempZoom <= 0.5f) {
            tempZoom = 0.5f;
        }
        else if (tempZoom >= 3.0f) {
            tempZoom = 3.0f;
        }

        transform.position = target.position + (offset * tempZoom);

        targetZoom += Input.mouseScrollDelta.y * -0.25f;



        // Quaternion rotation = Quaternion.Euler(targetRotation, 0.0f, 0.0f);

        // transform.rotation = rotation;

        transform.RotateAround(target.position, Vector3.up, targetRotation);
        transform.LookAt(target);

        if (Input.GetKey(KeyCode.LeftArrow)) {
            targetRotation -= Time.smoothDeltaTime * 60.0f;
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            targetRotation += Time.smoothDeltaTime * 60.0f;
        }

        zoom = Mathf.Lerp(zoom, targetZoom, Time.smoothDeltaTime * 5.0f);

        if (zoom <= 0.5f) {
            zoom = 0.5f;
        }

        if (zoom >= 3.0f) {
            zoom = 3.0f;
        }

        if (targetZoom <= 0.5f) {
            targetZoom = 0.5f;
        }

        if (targetZoom >= 3.0f) {
            targetZoom = 3.0f;
        }
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
