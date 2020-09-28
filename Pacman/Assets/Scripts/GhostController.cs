using Pathfinding;
using UnityEngine;
using System;

public class GhostController : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private Transform homeTransform;

    [SerializeField]
    private float nextWayPointDistance = 0f;

    [SerializeField]
    private GameState gameState;

    private Path path;

    private int currentWayPoint = 0;

    private Rigidbody2D rb;

    private Seeker seeker;

    public bool IsAlive;

    private Animator animator;

    private Vector2 lastDirection = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        seeker = gameObject.GetComponent<Seeker>();
        animator = gameObject.GetComponent<Animator>();
        rb.gravityScale = 0.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Physics2D.IgnoreLayerCollision(10, 9);
        Physics2D.IgnoreLayerCollision(10, 10);
        Physics2D.IgnoreLayerCollision(10, 11);
        Physics2D.IgnoreLayerCollision(11, 11);
        Physics2D.IgnoreLayerCollision(11, 9);
        IsAlive = true;
        InvokeRepeating("UpdatePath", 0, 0.5f);
    }

    private void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            Vector2 minDistance = p.vectorPath[0];
            int nearestWayPoint = 0;
            for(int i = 1; i < p.vectorPath.Count; i++)
            {
                if(Vector2.Distance(rb.position, p.vectorPath[i]) < Vector2.Distance(rb.position, minDistance))
                {
                    nearestWayPoint = i;
                }
            }
            currentWayPoint = nearestWayPoint;
        }
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            var targetPosition = IsAlive ? playerTransform.position : homeTransform.position;
            seeker.StartPath(rb.position, targetPosition, OnPathComplete);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null || currentWayPoint >= path.vectorPath.Count) return;

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 moveDirection;

        if (Math.Abs(direction.x) == Math.Abs(direction.y)) moveDirection = Vector2.zero;
        else if (Math.Abs(direction.x) > Math.Abs(direction.y))
        {
            if (direction.x > 0.0f)
            {
                if(lastDirection == Vector2.left)
                {
                    moveDirection = selectPosition(lastDirection);
                }
                else
                {
                    moveDirection = Vector2.right;
                }

            }
            else
            {
                if(lastDirection == Vector2.right)
                {
                    moveDirection = selectPosition(lastDirection);
                }
                else
                {
                    moveDirection = Vector2.left;
                }
            }

        }
        else
        {
            if (direction.y > 0.0f)
            {
                if (lastDirection == Vector2.down)
                {
                    moveDirection = selectPosition(lastDirection);
                }
                else
                {
                    moveDirection = Vector2.up;
                }

            }
            else
            {
                if(lastDirection == Vector2.up)
                {
                    moveDirection = selectPosition(lastDirection);
                }
                else
                {
                    moveDirection = Vector2.down;
                }
            }
        }

        print(moveDirection.ToString() + " - " + lastDirection.ToString());

        lastDirection = moveDirection;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        rb.velocity = moveDirection * 400 * Time.fixedDeltaTime;

        if (distance <= nextWayPointDistance)
        {
            currentWayPoint++;
        }

        animator.SetBool("IsAlive", IsAlive);
        animator.SetFloat("XSpeed", moveDirection.x);
        animator.SetFloat("YSpeed", moveDirection.y);
        animator.SetInteger("DangerLevel", gameState.isInvencible ? 2 : 0);
    }

    private Vector2 selectPosition(Vector2 currentDirection)
    {
        Vector2 result;
        if(currentDirection == Vector2.left)
        {
            if (!Physics2D.Raycast(transform.position - new Vector3(0.5f, 0.0f, 0.0f), Vector2.left, 0.5f)
            && !Physics2D.Raycast(transform.position - new Vector3(0.5f, -0.25f, 0.0f), Vector2.left, 0.5f)
            && !Physics2D.Raycast(transform.position - new Vector3(0.5f, 0.25f, 0.0f), Vector2.left, 0.5f)) result = Vector2.left;

            else if (!Physics2D.Raycast(transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector2.up, 0.5f)
            && !Physics2D.Raycast(transform.position + new Vector3(0.25f, 0.5f, 0.0f), Vector2.up, 0.5f)
            && !Physics2D.Raycast(transform.position + new Vector3(-0.25f, 0.5f, 0.0f), Vector2.up, 0.5f)) result = Vector2.up;

            else if (!Physics2D.Raycast(transform.position - new Vector3(0.0f, 0.5f, 0.0f), Vector2.down, 0.5f)
            && !Physics2D.Raycast(transform.position - new Vector3(0.25f, 0.5f, 0.0f), Vector2.down, 0.5f)
            && !Physics2D.Raycast(transform.position - new Vector3(-0.25f, 0.5f, 0.0f), Vector2.down, 0.5f)) result = Vector2.down;

            else result = Vector2.right;
        }
        else if(currentDirection == Vector2.right)
        {
            if (!Physics2D.Raycast(transform.position + new Vector3(0.5f, 0.0f, 0.0f), Vector2.right, 0.5f)
            && !Physics2D.Raycast(transform.position + new Vector3(0.5f, -0.25f, 0.0f), Vector2.right, 0.5f)
            && !Physics2D.Raycast(transform.position + new Vector3(0.5f, 0.25f, 0.0f), Vector2.right, 0.5f)) result = Vector2.right;

            else if (!Physics2D.Raycast(transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector2.up, 0.5f)
            && !Physics2D.Raycast(transform.position + new Vector3(0.25f, 0.5f, 0.0f), Vector2.up, 0.5f)
            && !Physics2D.Raycast(transform.position + new Vector3(-0.25f, 0.5f, 0.0f), Vector2.up, 0.5f)) result = Vector2.up;

            else if (!Physics2D.Raycast(transform.position - new Vector3(0.0f, 0.5f, 0.0f), Vector2.down, 0.5f)
            && !Physics2D.Raycast(transform.position - new Vector3(0.25f, 0.5f, 0.0f), Vector2.down, 0.5f)
            && !Physics2D.Raycast(transform.position - new Vector3(-0.25f, 0.5f, 0.0f), Vector2.down, 0.5f)) result = Vector2.down;

            else result = Vector2.left;
        }
        else if (currentDirection == Vector2.up)
        {
            if (!Physics2D.Raycast(transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector2.up, 0.5f)
            && !Physics2D.Raycast(transform.position + new Vector3(0.25f, 0.5f, 0.0f), Vector2.up, 0.5f)
            && !Physics2D.Raycast(transform.position + new Vector3(-0.25f, 0.5f, 0.0f), Vector2.up, 0.5f)) result = Vector2.up;

            else if (!Physics2D.Raycast(transform.position + new Vector3(0.5f, 0.0f, 0.0f), Vector2.right, 0.5f)
            && !Physics2D.Raycast(transform.position + new Vector3(0.5f, -0.25f, 0.0f), Vector2.right, 0.5f)
            && !Physics2D.Raycast(transform.position + new Vector3(0.5f, 0.25f, 0.0f), Vector2.right, 0.5f)) result = Vector2.right;

            else if (!Physics2D.Raycast(transform.position - new Vector3(0.5f, 0.0f, 0.0f), Vector2.left, 0.5f)
            && !Physics2D.Raycast(transform.position - new Vector3(0.5f, -0.25f, 0.0f), Vector2.left, 0.5f)
            && !Physics2D.Raycast(transform.position - new Vector3(0.5f, 0.25f, 0.0f), Vector2.left, 0.5f)) result = Vector2.left;

            else result = Vector2.down;
        }
        else if (currentDirection == Vector2.down)
        {
            if (!Physics2D.Raycast(transform.position - new Vector3(0.0f, 0.5f, 0.0f), Vector2.down, 0.5f)
            && !Physics2D.Raycast(transform.position - new Vector3(0.25f, 0.5f, 0.0f), Vector2.down, 0.5f)
            && !Physics2D.Raycast(transform.position - new Vector3(-0.25f, 0.5f, 0.0f), Vector2.down, 0.5f)) result = Vector2.down;

            else if (!Physics2D.Raycast(transform.position + new Vector3(0.5f, 0.0f, 0.0f), Vector2.right, 0.5f)
            && !Physics2D.Raycast(transform.position + new Vector3(0.5f, -0.25f, 0.0f), Vector2.right, 0.5f)
            && !Physics2D.Raycast(transform.position + new Vector3(0.5f, 0.25f, 0.0f), Vector2.right, 0.5f)) result = Vector2.right;

            else if (!Physics2D.Raycast(transform.position - new Vector3(0.5f, 0.0f, 0.0f), Vector2.left, 0.5f)
            && !Physics2D.Raycast(transform.position - new Vector3(0.5f, -0.25f, 0.0f), Vector2.left, 0.5f)
            && !Physics2D.Raycast(transform.position - new Vector3(0.5f, 0.25f, 0.0f), Vector2.left, 0.5f)) result = Vector2.left;

            else result = Vector2.up;
        }
        else
        {
            result = Vector2.zero;
        }

        return result;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameState.isInvencible)
        {
            IsAlive = false;
            gameObject.layer = 11;
            gameState.score += 200;
        }
    }


}
