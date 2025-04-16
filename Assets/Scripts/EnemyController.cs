using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator kgob;
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private bool isMovingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isMovingRight)
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);               //ÁÂ¿ì ÀÌµ¿
        else
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);

        kgob.SetBool("move", Mathf.Abs(rb.velocity.x) > 0.1f);

        if (rb.velocity.x > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (rb.velocity.x < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            isMovingRight = !isMovingRight;
        }
    }
}
