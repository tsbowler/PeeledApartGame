using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveLionWithBFS : MonoBehaviour
{
    public Transform monkey;
    public Tilemap obstaclesTilemap; // Reference to the obstacle tilemap
    public float moveSpeed = 1f;
    public float moveDistance = 1f;
    private Vector2 targetPosition;
    private bool isMoving = false;
    private Rigidbody2D rb;
    private Animator animator;  // Reference to the Animator

    void Start()
    {
        targetPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();  // Get the Animator component
    }

    void Update()
    {
        if (!isMoving)
        {
            // Perform BFS to find the shortest path to the monkey
            List<Vector2> path = FindPathBFS(transform.position, monkey.position);

            if (path != null && path.Count > 0)
            {
                // Set the next target position from the path
                targetPosition = path[0]; // Get the first position in the path
            }

            StartCoroutine(MoveLion());
        }
    }

    IEnumerator MoveLion()
    {
        isMoving = true;
        animator.SetBool("isWalking", true);  // Trigger walking animation

        while ((Vector2)transform.position != targetPosition)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime));
            
            // Optional: Flip the lion’s sprite direction based on movement
            if (targetPosition.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);  // Face right
            }
            else if (targetPosition.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);  // Face left
            }

            yield return null;  // Wait for the next frame
        }

        animator.SetBool("isWalking", false);  // Stop walking animation when movement is done
        isMoving = false;
    }

    // BFS Pathfinding
    List<Vector2> FindPathBFS(Vector2 start, Vector2 target)
    {
        Queue<Vector2> frontier = new Queue<Vector2>();
        frontier.Enqueue(start);

        Dictionary<Vector2, Vector2?> cameFrom = new Dictionary<Vector2, Vector2?>();
        cameFrom[start] = null;

        Vector2Int[] directions = {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        while (frontier.Count > 0)
        {
            Vector2 current = frontier.Dequeue();

            // Convert the world positions to tilemap grid cells for comparison
            Vector3Int currentCell = obstaclesTilemap.WorldToCell(current);
            Vector3Int targetCell = obstaclesTilemap.WorldToCell(target);

            // If we reached the target, reconstruct the path
            if (currentCell == targetCell)
            {
                return ReconstructPath(cameFrom, start, current);
            }

            // Explore neighbors
            foreach (Vector2Int direction in directions)
            {
                Vector2 neighbor = current + (Vector2)direction;

                // Check if the neighbor is walkable and hasn't been explored yet
                if (!cameFrom.ContainsKey(neighbor) && !IsObstacleAtPosition(neighbor))
                {
                    frontier.Enqueue(neighbor);
                    cameFrom[neighbor] = current; // Mark where we came from
                }
            }
        }

        return null; // No path found
    }

    // Reconstruct the path from the BFS search
    List<Vector2> ReconstructPath(Dictionary<Vector2, Vector2?> cameFrom, Vector2 start, Vector2 end)
    {
        List<Vector2> path = new List<Vector2>();
        Vector2? current = end;

        while (current != start)
        {
            path.Add(current.Value);
            current = cameFrom[current.Value];
        }

        path.Reverse(); // We need to reverse the path since we reconstructed it backwards
        return path;
    }

    // Function to check if a position is blocked by an obstacle
    bool IsObstacleAtPosition(Vector2 position)
    {
        Vector3Int cellPosition = obstaclesTilemap.WorldToCell(position);  // Convert world position to tilemap grid cell
        TileBase tile = obstaclesTilemap.GetTile(cellPosition);  // Get the tile at the target position
        return tile != null;  // Return true if there's a tile (i.e., an obstacle)
    }
}
