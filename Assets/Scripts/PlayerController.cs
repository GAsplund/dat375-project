using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private InfluenceStats influenceStats;

    private enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        influenceStats = FindObjectOfType<InfluenceStats>();
    }

    void FixedUpdate()
    {
        // These are supposed to work with both the arrow keys and WASD
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        if (movement.sqrMagnitude > 1f)
        {
            movement.Normalize();
        }

        rb.velocity = movement * moveSpeed;

        bool isMoving = movement.sqrMagnitude > Mathf.Epsilon;
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            Direction direction = GetDirection(movement);
            animator.SetInteger("Direction", (int)direction);

            if (direction == Direction.Up)
            {
                influenceStats.AddInfluence(1, InfluenceStats.InfluenceDirection.Right);
            }
            else if (direction == Direction.Down)
            {
                influenceStats.AddInfluence(1, InfluenceStats.InfluenceDirection.Left);
            }
        }
    }

    private Direction GetDirection(Vector2 movement)
    {
        // Prefer the dominant axis to determine facing direction
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            return movement.x > 0f ? Direction.Right : Direction.Left;
        }

        return movement.y > 0f ? Direction.Up : Direction.Down;
    }
}