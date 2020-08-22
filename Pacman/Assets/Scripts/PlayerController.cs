using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Vector3 moveDirection = Vector3.right * -1;

    private Rigidbody2D rb;

    public bool IsAlive;

    public Animator animator;

    private bool mvHoz = false;
    private bool mvVer = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        rb.gravityScale = 0.0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        IsAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAlive)
        {
            float xPos = 0, yPos = 0;
            if (Input.GetAxis("Vertical") != 0 && Input.GetAxis("Horizontal") != 0)
            {
                if (mvHoz)
                {
                    yPos = Input.GetAxis("Vertical");
                }
                else if (mvVer)
                {
                    xPos = Input.GetAxis("Horizontal");
                }
            }
            else
            {
                mvHoz = Input.GetAxis("Horizontal") != 0;
                xPos = Input.GetAxis("Horizontal");
                mvVer = Input.GetAxis("Vertical") != 0;
                yPos = Input.GetAxis("Vertical");
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
                moveDirection = Vector3.right * -1;
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
                moveDirection = Vector3.up * -1;
            }

            rb.velocity = moveDirection * 10;
            animator.SetBool("IsAlive", IsAlive);
            animator.SetFloat("XSpeed", moveDirection.x);
            animator.SetFloat("YSpeed", moveDirection.y);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

}