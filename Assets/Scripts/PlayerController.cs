using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Animator myAnimator;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private SpriteRenderer[] spriteRenderers;            //���������� ����� ����
    private Color originalColor;

    public float invincibilityDuration = 10f;     //�����ð�
    private bool isInvincible = false;

    private Rigidbody2D rb;
    private bool isGrounded;

    public TextMeshProUGUI invincibilityTimeText;

    private void Awake()    
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();            //���������� ����� ����
        if (spriteRenderers.Length > 0)
            originalColor = spriteRenderers[0].color;
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

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            myAnimator.SetTrigger("JumpAction1");

        }


        transform.Translate(Vector3.right*direction*moveSpeed*Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            if (!isInvincible)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
              // ���� ������ �� �� ���� �� ��
        }
        else if (collision.CompareTag("InvincibilityItem"))
        {
            Destroy(collision.gameObject); // ������ ����
            StartCoroutine(ActivateInvincibility());
        }

        if (collision.CompareTag("Finish"))
        {
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        if (collision.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }

    private System.Collections.IEnumerator ActivateInvincibility()
    {
        isInvincible = true;

        // ��� ������ ������ ����� ����
        foreach (var sr in spriteRenderers)
        {
            sr.color = new Color(1f, 1f, 0f, 0.5f);
        }

        // ���� �ð� ǥ��
        float remainingTime = invincibilityDuration;
        while (remainingTime > 0)
        {
            invincibilityTimeText.text = "���� �ð�: " + Mathf.Ceil(remainingTime).ToString() + "s";
            remainingTime -= Time.deltaTime;
            yield return null;  // �� ������ ��ٸ���
        }

        invincibilityTimeText.text = "";  // �ð��� ������ �ؽ�Ʈ ����

        // ���� ������ ����
        foreach (var sr in spriteRenderers)
        {
            sr.color = originalColor;
        }

        isInvincible = false;
    }

}
