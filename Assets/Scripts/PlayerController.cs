using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator myAnimator;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator pAni;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
    }
    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput < 0)
            transform.localScale = new Vector3(1f, 1f, 1f);

        if (moveInput > 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            pAni.SetTrigger("JumpAction");
        }

        float direction = Input.GetAxis("Horizontal");

        if (direction > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            myAnimator.SetBool("move", true);
        }
        else if (direction < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            myAnimator.SetBool("move", true);
        }
        else
            myAnimator.SetBool("move", false);

        transform.Translate(Vector3.right*direction*moveSpeed*Time.deltaTime);
    }
}
