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

    private SpriteRenderer[] spriteRenderers;            //무적아이템 노란색 변수
    private Color originalColor;

    public float invincibilityDuration = 10f;     //무적시간
    private bool isInvincible = false;

    public float speedBoostMultiplier = 2f; // 속도 배수 (예: 2배)
    public float speedBoostDuration = 5f;   // 지속 시간

    private bool isSpeedBoosted = false;

    public float jumpBoostMultiplier = 1.5f;   // 점프력 몇 배 증가
    public float jumpBoostDuration = 5f;       // 점프력 증가 지속 시간

    private bool isJumpBoosted = false;

    private Rigidbody2D rb;
    private bool isGrounded;

    public TextMeshProUGUI invincibilityTimeText;

    private void Awake()    
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();            //무적아이템 노란색 변수
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
              // 무적 상태일 땐 씬 리셋 안 함
        }
        else if (collision.CompareTag("InvincibilityItem"))
        {
            Destroy(collision.gameObject); // 아이템 제거
            StartCoroutine(ActivateInvincibility());
        }

        if (collision.CompareTag("Finish"))
        {
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        if (collision.CompareTag("Enemy"))
        {

            if (!isInvincible)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        else if (collision.CompareTag("SpeedItem"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(ActivateSpeedBoost());
        }

        else if (collision.CompareTag("JumpItem"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(ActivateJumpBoost());
        }

    }

    private System.Collections.IEnumerator ActivateInvincibility()
    {
        isInvincible = true;

        // 모든 파츠에 반투명 노란색 적용
        foreach (var sr in spriteRenderers)
        {
            sr.color = new Color(1f, 1f, 0f, 0.5f);
        }

        // 무적 시간 표시
        float remainingTime = invincibilityDuration;
        while (remainingTime > 0)
        {
            invincibilityTimeText.text = "무적 시간: " + Mathf.Ceil(remainingTime).ToString() + "s";
            remainingTime -= Time.deltaTime;
            yield return null;  // 한 프레임 기다리기
        }

        invincibilityTimeText.text = "";  // 시간이 끝나면 텍스트 비우기

        // 원래 색으로 복원
        foreach (var sr in spriteRenderers)
        {
            sr.color = originalColor;
        }

        isInvincible = false;
    }

        private System.Collections.IEnumerator ActivateSpeedBoost()
    { 
        if (isSpeedBoosted) yield break; // 중복 방지

        isSpeedBoosted = true;
        moveSpeed *= speedBoostMultiplier;

        // 속도 증가 시간동안 대기
        float speedRemainingTime = speedBoostDuration;
        while (speedRemainingTime > 0)
        {
            invincibilityTimeText.text = $"속도 업: {Mathf.Ceil(speedRemainingTime)}s";
            speedRemainingTime -= Time.deltaTime;
            yield return null;
        }

        moveSpeed /= speedBoostMultiplier;
        invincibilityTimeText.text = "";
        isSpeedBoosted = false;

    }

    private System.Collections.IEnumerator ActivateJumpBoost()
    {
        if (isJumpBoosted) yield break; // 중복 방지

        isJumpBoosted = true;
        jumpForce *= jumpBoostMultiplier;

        float jumpRemainingTime = jumpBoostDuration;
        while (jumpRemainingTime > 0)
        {
            invincibilityTimeText.text = $"점프 업: {Mathf.Ceil(jumpRemainingTime)}s";
            jumpRemainingTime -= Time.deltaTime;
            yield return null;
        }

        jumpForce /= jumpBoostMultiplier;
        invincibilityTimeText.text = "";
        isJumpBoosted = false;
    }

}
