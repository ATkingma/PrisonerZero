using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Speed of the camera smoothing
    public Vector3 offset; // Offset from the player

    void FixedUpdate()
    {
        Vector3 desiredPosition = player.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}
