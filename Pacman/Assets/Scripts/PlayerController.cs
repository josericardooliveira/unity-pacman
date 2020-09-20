using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Vector3 moveDirection = Vector3.right * -1;

    private Rigidbody2D rb;

    public bool IsAlive;

    private Animator animator;

    private bool mvHoz = false;
    private bool mvVer = false;

    private Transform playerDirection;

    [SerializeField]
    private GameState gameState;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        playerDirection = transform.GetChild(0);
        rb.gravityScale = 0.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Physics2D.IgnoreLayerCollision(12, 11);
        IsAlive = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsAlive)
        {
            float xPos = 0, yPos = 0;

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (Mathf.Abs(horizontal) < 0.3f) horizontal = 0.0f;
            if (Mathf.Abs(vertical) < 0.3f) vertical = 0.0f;

            if (vertical != 0 && horizontal != 0)
            {
                if (mvHoz)
                {
                    yPos = vertical;
                }
                else if (mvVer)
                {
                    xPos = horizontal;
                }
            }
            else
            {
                mvHoz = horizontal != 0;
                xPos = horizontal;
                mvVer = vertical != 0;
                yPos = vertical;
            }

            if (xPos > 0
                && !Physics2D.Raycast(transform.position + new Vector3(0.5f, 0.0f, 0.0f), Vector2.right, 0.5f)
                && !Physics2D.Raycast(transform.position + new Vector3(0.5f, -0.25f, 0.0f), Vector2.right, 0.5f)
                && !Physics2D.Raycast(transform.position + new Vector3(0.5f, 0.25f, 0.0f), Vector2.right, 0.5f))
            {
                moveDirection = Vector3.right;
                
            }
            else if (xPos < 0
                && !Physics2D.Raycast(transform.position - new Vector3(0.5f, 0.0f, 0.0f), Vector2.left, 0.5f)
                && !Physics2D.Raycast(transform.position - new Vector3(0.5f, -0.25f, 0.0f), Vector2.left, 0.5f)
                && !Physics2D.Raycast(transform.position - new Vector3(0.5f, 0.25f, 0.0f), Vector2.left, 0.5f))
            {
                moveDirection = Vector3.left;
            }
            else if (yPos > 0
                && !Physics2D.Raycast(transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector2.up, 0.5f)
                && !Physics2D.Raycast(transform.position + new Vector3(0.25f, 0.5f, 0.0f), Vector2.up, 0.5f)
                && !Physics2D.Raycast(transform.position + new Vector3(-0.25f, 0.5f, 0.0f), Vector2.up, 0.5f))
            {
                moveDirection = Vector3.up;
            }
            else if (yPos < 0
                && !Physics2D.Raycast(transform.position - new Vector3(0.0f, 0.5f, 0.0f), Vector2.down, 0.5f)
                && !Physics2D.Raycast(transform.position - new Vector3(0.25f, 0.5f, 0.0f), Vector2.down, 0.5f)
                && !Physics2D.Raycast(transform.position - new Vector3(-0.25f, 0.5f, 0.0f), Vector2.down, 0.5f))
            {
                moveDirection = Vector3.down;
            }

            playerDirection.forward = moveDirection;

            rb.velocity = moveDirection * 400 * Time.fixedDeltaTime;

            animator.SetFloat("XSpeed", moveDirection.x);
            animator.SetFloat("YSpeed", moveDirection.y);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        animator.SetBool("IsAlive", IsAlive);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && !gameState.isInvencible)
        {
            IsAlive = false;
            rb.simulated = false;
        }
    }

}