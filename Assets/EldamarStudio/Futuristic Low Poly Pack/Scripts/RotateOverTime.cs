using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    public Vector3 Direction;
    public float RotationSpeed = 0.025f;

    void Update()
    {
        transform.Rotate(Direction * (RotationSpeed * Time.deltaTime));
    }
}