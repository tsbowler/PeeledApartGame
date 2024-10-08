using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveMonkey : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveSpeed = 1f;
    private Vector2 targetPosition;
    private Animator animator;

    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if ((Vector2)transform.position == targetPosition)
        {
            if (Input.GetKey(KeyCode.W))
            {
                targetPosition += Vector2.up * moveDistance;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                targetPosition += Vector2.down * moveDistance;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                targetPosition += Vector2.left * moveDistance;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                targetPosition += Vector2.right * moveDistance;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveDistance * moveSpeed * Time.deltaTime);

        bool isWalking = (Vector2)transform.position != targetPosition; // not yet functioning
        animator.SetBool("isWalking", isWalking);
    }
}

