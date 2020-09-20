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
            if (direction.x > 0.0f) moveDirection = Vector2.right;
            else moveDirection = Vector2.left;
        }
        else
        {
            if (direction.y > 0.0f) moveDirection = Vector2.up;
            else moveDirection = Vector2.down;
        }

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
