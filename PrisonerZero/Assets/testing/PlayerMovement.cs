using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player
    public Rigidbody2D rb; // Reference to the Rigidbody2D component

    private Vector2 movement; // Variable to store the movement input

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component if not assigned
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from the user
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Store the input in the movement vector
        movement = new Vector2(moveX, moveY);
    }

    // FixedUpdate is called a fixed number of times per second
    void FixedUpdate()
    {
        // Move the player by applying force to the Rigidbody2D
        rb.velocity = movement * moveSpeed;
    }
}
