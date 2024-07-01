using UnityEngine;

public class RotateScript : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 100f; // Speed of rotation in degrees per second

    [SerializeField]
    private Vector3 rotationAxis = Vector3.forward; // Axis of rotation

    void Update()
    {
        // Calculate the rotation for this frame
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Apply the rotation to the GameObject
        transform.Rotate(rotationAxis, rotationAmount);
    }
}
