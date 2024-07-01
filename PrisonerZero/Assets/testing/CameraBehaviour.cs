using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Speed of the camera smoothing
    public Vector3 offset; // Offset from the player
    public float mouseInfluence = 0.1f; // Amount of influence the mouse has on camera movement

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void FixedUpdate()
    {
        Vector3 playerPosition = player.position + offset;

        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; 

        Vector3 influencedPosition = playerPosition + (mousePosition - playerPosition) * mouseInfluence;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, influencedPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}
