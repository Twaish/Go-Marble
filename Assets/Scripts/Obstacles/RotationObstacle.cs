using UnityEngine;

public class ObstacleRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    public Transform rotationCenter;      // The point to rotate around
    public Vector3 rotationAxis = Vector3.up; // Axis to rotate around (e.g., Y-axis)
    public float rotationSpeed = 30f;     // Degrees per second

    void Update()
    {
        if (rotationCenter != null)
        {
            // Rotate the obstacle around the given point and axis
            transform.RotateAround(rotationCenter.position, rotationAxis, rotationSpeed * Time.deltaTime);
        }
    }
}

