using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveLionWithBFS : MonoBehaviour
{
    public Transform monkey;
    public Tilemap obstaclesTilemap;
    public float normalMoveSpeed = 1f;
    public float lineOfSightMoveSpeed = 2f;
    public float moveDistance = 1f;
    public LayerMask obstacleLayerMask;
    private Vector2 targetPosition;
    private bool isMoving = false;
    private Rigidbody2D rb;
    public Animator animator;
    private float currentMoveSpeed;

    void Start()
    {
        targetPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentMoveSpeed = normalMoveSpeed;
    }

    void Update()
    {
        if (HasLineOfSight(monkey.position))
        {
            currentMoveSpeed = lineOfSightMoveSpeed * moveDistance;
            animator.speed = 2f;
        }
        else
        {
            currentMoveSpeed = normalMoveSpeed * moveDistance;
            animator.speed = 1f;
        }

        if (!isMoving)
        {
            List<Vector2> path = FindPathBFS(transform.position, monkey.position);

            if (path != null && path.Count > 0)
            {
                targetPosition = path[0];
            }

            StartCoroutine(MoveLion());
        }
    }

    IEnumerator MoveLion()
    {
        isMoving = true;
        animator.SetBool("isWalking", true);

        while ((Vector2)transform.position != targetPosition)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, currentMoveSpeed * Time.fixedDeltaTime));
            
            if (targetPosition.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (targetPosition.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            yield return null;
        }

        animator.SetBool("isWalking", false);
        isMoving = false;
    }

    bool HasLineOfSight(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;
        float distance = direction.magnitude;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, obstacleLayerMask);
        return hit.collider == null;
    }

    List<Vector2> FindPathBFS(Vector2 start, Vector2 target)
    {
        Queue<Vector2> frontier = new Queue<Vector2>();
        frontier.Enqueue(start);

        Dictionary<Vector2, Vector2?> cameFrom = new Dictionary<Vector2, Vector2?>();
        cameFrom[start] = null;

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (frontier.Count > 0)
        {
            Vector2 current = frontier.Dequeue();

            Vector3Int currentCell = obstaclesTilemap.WorldToCell(current);
            Vector3Int targetCell = obstaclesTilemap.WorldToCell(target);

            if (currentCell == targetCell)
            {
                return ReconstructPath(cameFrom, start, current);
            }

            foreach (Vector2Int direction in directions)
            {
                Vector2 neighbor = current + (Vector2)direction;

                if (!cameFrom.ContainsKey(neighbor) && !IsObstacleAtPosition(neighbor))
                {
                    frontier.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        return null;
    }

    List<Vector2> ReconstructPath(Dictionary<Vector2, Vector2?> cameFrom, Vector2 start, Vector2 end)
    {
        List<Vector2> path = new List<Vector2>();
        Vector2? current = end;

        while (current != start)
        {
            path.Add(current.Value);
            current = cameFrom[current.Value];
        }

        path.Reverse();
        return path;
    }

    bool IsObstacleAtPosition(Vector2 position)
    {
        Vector3Int cellPosition = obstaclesTilemap.WorldToCell(position);
        TileBase tile = obstaclesTilemap.GetTile(cellPosition);
        return tile != null;
    }
}
