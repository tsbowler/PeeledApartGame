using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveLion : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 1f;
    public Tilemap obstaclesTilemap;  // Reference to the obstacle tilemap
    public Transform monkeyTransform; // Reference to the monkey's position
    private Vector2 targetPosition;
    private Rigidbody2D rb;

    void Start()
    {
        targetPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if ((Vector2)transform.position == targetPosition)
        {
            Vector2 newPosition = ChooseDirection();
            if (!IsObstacleAtPosition(newPosition))
            {
                targetPosition = newPosition;
            }
        }

        rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, moveDistance * moveSpeed * Time.fixedDeltaTime));
    }

    // Choose direction based on monkey's position, prioritizing the closest axis
    Vector2 ChooseDirection()
    {
        Vector2 currentPos = transform.position;
        Vector2 monkeyPos = monkeyTransform.position;

        Vector2 direction = Vector2.zero;

        float xDistance = monkeyPos.x - currentPos.x;
        float yDistance = monkeyPos.y - currentPos.y;

        // Prioritize the axis with the larger distance
        if (Mathf.Abs(xDistance) > Mathf.Abs(yDistance))
        {
            // Try moving on the X-axis first
            direction = xDistance > 0 ? Vector2.right * moveDistance : Vector2.left * moveDistance;
            if (!IsObstacleAtPosition(currentPos + direction))
            {
                return currentPos + direction;
            }

            // If X-axis movement fails, try the Y-axis
            direction = yDistance > 0 ? Vector2.up * moveDistance : Vector2.down * moveDistance;
            if (!IsObstacleAtPosition(currentPos + direction))
            {
                return currentPos + direction;
            }

            // If both X and Y fail, try moving in the opposite X direction
            direction = xDistance > 0 ? Vector2.left * moveDistance : Vector2.right * moveDistance;
            if (!IsObstacleAtPosition(currentPos + direction))
            {
                return currentPos + direction;
            }

            // Finally, if everything fails, try the opposite Y direction
            return currentPos + (yDistance > 0 ? Vector2.down * moveDistance : Vector2.up * moveDistance);
        }
        else
        {
            // Try moving on the Y-axis first
            direction = yDistance > 0 ? Vector2.up * moveDistance : Vector2.down * moveDistance;
            if (!IsObstacleAtPosition(currentPos + direction))
            {
                return currentPos + direction;
            }

            // If Y-axis movement fails, try the X-axis
            direction = xDistance > 0 ? Vector2.right * moveDistance : Vector2.left * moveDistance;
            if (!IsObstacleAtPosition(currentPos + direction))
            {
                return currentPos + direction;
            }

            // If both Y and X fail, try moving in the opposite Y direction
            direction = yDistance > 0 ? Vector2.down * moveDistance : Vector2.up * moveDistance;
            if (!IsObstacleAtPosition(currentPos + direction))
            {
                return currentPos + direction;
            }

            // Finally, if everything fails, try the opposite X direction
            return currentPos + (xDistance > 0 ? Vector2.left * moveDistance : Vector2.right * moveDistance);
        }
    }

    // Function to check if the target position is blocked by an obstacle
    bool IsObstacleAtPosition(Vector2 position)
    {
        Vector3Int cellPosition = obstaclesTilemap.WorldToCell(position);  // Convert world position to tilemap grid cell
        TileBase tile = obstaclesTilemap.GetTile(cellPosition);  // Get the tile at the target position
        return tile != null;  // Return true if there's a tile (i.e., an obstacle)
    }
}
