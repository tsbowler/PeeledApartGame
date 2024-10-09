using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveMonkey : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 1f;
    public Tilemap obstaclesTilemap;  // Reference to the obstacle tilemap
    private Vector2 targetPosition;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if ((Vector2)transform.position == targetPosition)
        {
            Vector2 newPosition = targetPosition;

            if (Input.GetKey(KeyCode.W))
            {
                newPosition += Vector2.up * moveDistance;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                newPosition += Vector2.down * moveDistance;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                newPosition += Vector2.left * moveDistance;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                newPosition += Vector2.right * moveDistance;
                transform.localScale = new Vector3(1, 1, 1);
            }

            // Check if the newPosition is walkable (not an obstacle)
            if (!IsObstacleAtPosition(newPosition))
            {
                targetPosition = newPosition;  // Allow movement if no obstacle
            }
        }

        rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, moveDistance * moveSpeed * Time.fixedDeltaTime));

        bool isWalking = (Vector2)transform.position != targetPosition;
        animator.SetBool("isWalking", isWalking);
    }

    // Function to check if the target position is blocked by an obstacle
    bool IsObstacleAtPosition(Vector2 position)
    {
        Vector3Int cellPosition = obstaclesTilemap.WorldToCell(position);  // Convert world position to tilemap grid cell
        TileBase tile = obstaclesTilemap.GetTile(cellPosition);  // Get the tile at the target position
        return tile != null;  // Return true if there's a tile (i.e., an obstacle)
    }
}

