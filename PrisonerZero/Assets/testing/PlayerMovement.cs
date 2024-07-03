using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private List<SpriteRenderer> spriteRenderer = new List<SpriteRenderer>();


    private Vector2 movement;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        UpgradeManager.Instance.SetPlayerStats(moveSpeed, 100, 10);
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        movement = new Vector2(moveX, moveY);

        if (movement.x < 0)
        {
            foreach(SpriteRenderer r in spriteRenderer)
            {
                r.flipX = true;
            }
        }
        else if (movement.x > 0)
        {
            foreach (SpriteRenderer r in spriteRenderer)
            {
                r.flipX = false;
            }
        }
    }

    void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }

    public void SetMovementSpeed(float movementSpeed)
    {
        this.moveSpeed = movementSpeed;
    }
}
