using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMove : MonoBehaviour
{
    public Rigidbody2D rigid;
    public GameObject player;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public float detectionRange = 5f;      // �÷��̾ �����ϴ� �Ÿ�
    public float jumpForce = 9f;           // ���� ��
    public float moveSpeed = 4f;           // �̵� �ӵ�

    private bool isGrounded = true;
    private bool isJumping = false;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(player.transform.position, transform.position);   // �÷��̾���� �Ÿ�

            // �÷��̾ ���� ���� ���� ���� �� (���� ����)
            if (distance <= detectionRange && isGrounded && !isJumping)
            {
                // �÷��̾� ������ ���� ��ȯ
                spriteRenderer.flipX = !(player.transform.position.x < transform.position.x);
                StartCoroutine(Jump());
            }
        }

        // ���� ���θ� ���������� üũ
        CheckGrounded();
    }

    // �÷��̾ ���� �����ϴ� �Լ�
    IEnumerator Jump()
    {
        yield return new WaitForSeconds(0.3f);
        isGrounded = false;
        isJumping = true;
        animator.SetBool("jumping", true);

        // �÷��̾� �������� ����
        Vector3 direction = (player.transform.position - transform.position).normalized;
        rigid.velocity = new Vector2(direction.x * moveSpeed, jumpForce);
    }

    // ���� ���θ� üũ�ϴ� �Լ�
    void CheckGrounded()
    {
        Debug.DrawRay(rigid.position, Vector3.down, Color.yellow);
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Ground"));
        if (rayHit.collider != null)
        {
            if (rayHit.distance < 1.3f)
            {
                isGrounded = true;
                isJumping = false;
                //animator.SetBool("jumping", false);
                animator.SetBool("falling", false);

                // ���� �� 1�� ��� �� �ٽ� ���� ����
                StartCoroutine(WaitForNextJump());
            }
        }
        else
        {
            isGrounded = false;
            if (rigid.velocity.y <= 0)
            {
                animator.SetBool("jumping", false);
                animator.SetBool("falling", true);
            }
                
        }
    }

    // ���� �� 1�� ����ϴ� �Լ�
    IEnumerator WaitForNextJump()
    {
        yield return new WaitForSeconds(1f);  // 1�� ���
    }
}
