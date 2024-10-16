using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveMonkey : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 1f;
    public bool isFlying = false;
    private float speedDefault;
    public Tilemap obstaclesTilemap;
    private Vector2 targetPosition;
    private Animator animator;
    private Rigidbody2D rb;
    

    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        speedDefault = moveSpeed;
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

            if (!IsObstacleAtPosition(newPosition) || isFlying)
            {
                targetPosition = newPosition;
            }
            if (isFlying)
                    animator.SetBool("isFlying", true);
                else
                    animator.SetBool("isFlying", false);
            
        }

        rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, moveDistance * moveSpeed * Time.fixedDeltaTime));

        if ((moveSpeed != speedDefault) && ((Vector2)transform.position != targetPosition))
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
            bool isWalking = (Vector2)transform.position != targetPosition;
            animator.SetBool("isWalking", isWalking);
        }
    }

    bool IsObstacleAtPosition(Vector2 position)
    {
        Vector3Int cellPosition = obstaclesTilemap.WorldToCell(position);
        TileBase tile = obstaclesTilemap.GetTile(cellPosition);
        return tile != null;  
    }

    public void UpdateTargetPosition(Vector2 newTargetPosition)
    {
        targetPosition = newTargetPosition;
    }
}
