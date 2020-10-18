using UnityEngine;
using System;

public enum GhostAIState
{
    CHASE,
    SCATTER,
    EATEN,
    FRIGHTENED,
    FRIGHTENED_FINISHING
}

public class GhostController : MonoBehaviour
{

    [SerializeField]
    private Transform homeTransform;

    [SerializeField]
    private Transform ghostHouseDoor;

    [SerializeField]
    private Transform cornerTransform;

    [SerializeField]
    private GameState gameState;

    private TargetProvider targetProvider;

    private Rigidbody2D rb;

    public GhostAIState aiState;

    private Animator animator;

    private Vector2 lastDirection = Vector2.zero;

    private Vector3 lastDirectionChangePosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        targetProvider = gameObject.GetComponent<TargetProvider>();
        rb.gravityScale = 0.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Physics2D.IgnoreLayerCollision(13, 9);
        Physics2D.IgnoreLayerCollision(10, 10);
        Physics2D.IgnoreLayerCollision(10, 11);
        Physics2D.IgnoreLayerCollision(11, 11);
        Physics2D.IgnoreLayerCollision(11, 9);
        Physics2D.IgnoreLayerCollision(10, 13);
        aiState = GhostAIState.SCATTER;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float rayDistance = 0.05f;

        float innerMargin = 0.5f;

        float sideMargin = 0.25f;


        LayerMask mask;

        if(gameObject.layer == 10)
        {
            mask = 1 << LayerMask.NameToLayer("Block") | 1 << LayerMask.NameToLayer("Door");
        }
        else
        {
            mask = 1 << LayerMask.NameToLayer("Block");
        }


        Vector3 upBase = transform.position + Vector3.up * innerMargin;
        Vector3 upLeft = transform.position + new Vector3(-sideMargin, innerMargin, 0.0f);
        Vector3 upRight = transform.position + new Vector3(sideMargin, innerMargin, 0.0f);

        Vector3 downBase = transform.position + Vector3.down * innerMargin;
        Vector3 downLeft = transform.position + new Vector3(-sideMargin, -innerMargin, 0.0f);
        Vector3 downRight = transform.position + new Vector3(sideMargin, -innerMargin, 0.0f);

        Vector3 leftBase = transform.position + Vector3.left * innerMargin;
        Vector3 leftUp = transform.position + new Vector3(-innerMargin, sideMargin, 0.0f);
        Vector3 leftDown = transform.position + new Vector3(-innerMargin, -sideMargin, 0.0f);

        Vector3 rightBase = transform.position + Vector3.right * innerMargin;
        Vector3 rightUp = transform.position + new Vector3(innerMargin, sideMargin, 0.0f);
        Vector3 rightDown = transform.position + new Vector3(innerMargin, -sideMargin, 0.0f);

        bool isUpAvailable = !Physics2D.Raycast(upBase, Vector2.up, rayDistance, mask)
            && !Physics2D.Raycast(upLeft, Vector2.up, rayDistance, mask)
            && !Physics2D.Raycast(upRight, Vector2.up, rayDistance, mask);

        bool isLeftAvailable = !Physics2D.Raycast(leftBase, Vector2.left, rayDistance, mask)
            && !Physics2D.Raycast(leftUp, Vector2.left, rayDistance, mask)
            && !Physics2D.Raycast(leftDown, Vector2.left, rayDistance, mask);

        bool isDownAvailable = !Physics2D.Raycast(downBase, Vector2.down, rayDistance, mask)
            && !Physics2D.Raycast(downLeft, Vector2.down, rayDistance, mask)
            && !Physics2D.Raycast(downRight, Vector2.down, rayDistance, mask);

        bool isRightAvailable = !Physics2D.Raycast(rightBase, Vector2.right, rayDistance, mask)
            && !Physics2D.Raycast(rightUp, Vector2.right, rayDistance, mask)
            && !Physics2D.Raycast(rightDown, Vector2.right, rayDistance, mask);

        //float debugFactor = 20.0f;

        //if (!Physics2D.Raycast(upBase, Vector2.up, rayDistance, mask))
        //    Debug.DrawRay(upBase, Vector3.up * rayDistance * debugFactor, Color.green, Time.fixedDeltaTime);
        //if (!Physics2D.Raycast(upLeft, Vector2.up, rayDistance, mask))
        //    Debug.DrawRay(upLeft, Vector3.up * rayDistance * debugFactor, Color.green, Time.fixedDeltaTime);
        //if (!Physics2D.Raycast(upRight, Vector2.up, rayDistance, mask))
        //    Debug.DrawRay(upRight, Vector3.up * rayDistance * debugFactor, Color.green, Time.fixedDeltaTime);

        //if (!Physics2D.Raycast(leftBase, Vector2.left, rayDistance, mask))
        //    Debug.DrawRay(leftBase, Vector2.left * rayDistance * debugFactor, Color.red, Time.fixedDeltaTime);
        //if (!Physics2D.Raycast(leftUp, Vector2.left, rayDistance, mask))
        //    Debug.DrawRay(leftUp, Vector2.left * rayDistance * debugFactor, Color.red, Time.fixedDeltaTime);
        //if (!Physics2D.Raycast(leftDown, Vector2.left, rayDistance, mask))
        //    Debug.DrawRay(leftDown, Vector2.left * rayDistance * debugFactor, Color.red, Time.fixedDeltaTime);

        //if (!Physics2D.Raycast(downBase, Vector2.down, rayDistance, mask))
        //    Debug.DrawRay(downBase, Vector2.down * rayDistance * debugFactor, Color.blue, Time.fixedDeltaTime);
        //if (!Physics2D.Raycast(downLeft, Vector2.down, rayDistance, mask))
        //    Debug.DrawRay(downLeft, Vector2.down * rayDistance * debugFactor, Color.blue, Time.fixedDeltaTime);
        //if (!Physics2D.Raycast(downRight, Vector2.down, rayDistance, mask))
        //    Debug.DrawRay(downRight, Vector2.down * rayDistance * debugFactor, Color.blue, Time.fixedDeltaTime);

        //if (!Physics2D.Raycast(rightBase, Vector2.right, rayDistance, mask))
        //    Debug.DrawRay(rightBase, Vector2.right * rayDistance * debugFactor, Color.yellow, Time.fixedDeltaTime);
        //if (!Physics2D.Raycast(rightUp, Vector2.right, rayDistance, mask))
        //    Debug.DrawRay(rightUp, Vector2.right * rayDistance * debugFactor, Color.yellow, Time.fixedDeltaTime);
        //if (!Physics2D.Raycast(rightDown, Vector2.right, rayDistance, mask))
        //    Debug.DrawRay(rightDown, Vector2.right * rayDistance * debugFactor, Color.yellow, Time.fixedDeltaTime);

        Vector3 targetPosition;
        
        switch(gameObject.layer)
        {
            case 11:
                targetPosition = homeTransform.position;
                break;
            case 13:
                targetPosition = ghostHouseDoor.position;
                break;
            default:
                if(aiState == GhostAIState.CHASE)
                {
                    targetPosition = targetProvider.getTargetPosition();
                }
                else
                {
                    targetPosition = cornerTransform.position;
                }
                
                break;
        }

        //Debug.DrawRay(transform.position, targetPosition - transform.position, Color.yellow);

        float distanceUp = Math.Abs(Vector3.Distance(transform.position + Vector3.up * rayDistance, targetPosition)) + (lastDirection == Vector2.down || !isUpAvailable ? float.MaxValue : 0.0f);
        float distanceLeft = Math.Abs(Vector3.Distance(transform.position + Vector3.left * rayDistance, targetPosition)) + (lastDirection == Vector2.right || !isLeftAvailable ? float.MaxValue : 0.0f);
        float distanceDown = Math.Abs(Vector3.Distance(transform.position + Vector3.down * rayDistance, targetPosition)) + (lastDirection == Vector2.up || !isDownAvailable ? float.MaxValue : 0.0f);
        float distanceRight = Math.Abs(Vector3.Distance(transform.position + Vector3.right * rayDistance, targetPosition)) + (lastDirection == Vector2.left || !isRightAvailable ? float.MaxValue : 0.0f);

        float[] distances = { distanceUp, distanceLeft, distanceDown, distanceRight };

        float minValue = Mathf.Min(distances);

        Vector2 moveDirection;

        if (Math.Abs(Vector3.Distance(transform.position, lastDirectionChangePosition)) > 0.5f || lastDirection == Vector2.zero)
        {
            if (minValue == distanceUp) moveDirection = Vector2.up;
            else if (minValue == distanceLeft) moveDirection = Vector2.left;
            else if (minValue == distanceDown) moveDirection = Vector2.down;
            else moveDirection = Vector2.right;
        }
        else
        {
            moveDirection = lastDirection;
        }


        if(lastDirection != moveDirection)
        {
            //if (moveDirection == Vector2.up) print("UP");
            //else if (moveDirection == Vector2.down) print("DOWN");
            //else if (moveDirection == Vector2.right) print("RIGHT");
            //else if (moveDirection == Vector2.left) print("LEFT");
            //else print("NONE");
            lastDirectionChangePosition = transform.position;
        }

        lastDirection = moveDirection;

        rb.velocity = moveDirection * 400 * Time.fixedDeltaTime;

        animator.SetFloat("XSpeed", moveDirection.x);
        animator.SetFloat("YSpeed", moveDirection.y);
        animator.SetInteger("GhostState", (int)aiState);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")
            && (aiState == GhostAIState.FRIGHTENED || aiState == GhostAIState.FRIGHTENED_FINISHING))
        {
            aiState = GhostAIState.EATEN;
            gameObject.layer = 11;
            gameState.score += 200;
        }
    }

    public void Reset()
    {
        lastDirection = Vector2.zero;
    }


}
