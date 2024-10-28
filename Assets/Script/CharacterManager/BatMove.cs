using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public Rigidbody2D rigid;
    public GameObject player;

    public float detectionRange = 6f;      // �÷��̾ �����ϴ� �Ÿ�
    public float stopChasingRange = 7f;    // ������ ���ߴ� �Ÿ�
    public float flySpeed = 2f;            // ���ư��� �ӵ�
    
    private bool isChasing = false;        // ���� ���� �÷���
    private Animator animator;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        CheckPlayerDistance();
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            // �Ŵ޷� ���� ���� �������� ����
            rigid.velocity = Vector2.zero;
        }
    }

    // �÷��̾���� �Ÿ��� üũ�Ͽ� ���� ���¸� �����ϴ� �Լ�
    void CheckPlayerDistance()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(player.transform.position, transform.position);

            // ���� ���� ���� (�÷��̾ ������� ��)
            if (distance < detectionRange && !isChasing)
            {
                isChasing = true;
                animator.SetBool("flying", true); 
            }
            // ���� ���� ���� (�÷��̾ �־��� ��)
            else if (distance >= stopChasingRange && isChasing)
            {
                isChasing = false;
            }
        }
    }

    // �÷��̾ �����ϴ� �Լ�
    void ChasePlayer()
    {
        if (player != null)
        {
            // �÷��̾ ���� ���� ���� ���
            Vector3 direction = (player.transform.position - transform.position).normalized;

            // ���⿡ ���� ��������Ʈ�� �¿�� ������
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false;  // �÷��̾ �����ʿ� ���� ��
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = true;   // �÷��̾ ���ʿ� ���� ��
            }

            // �÷��̾� �������� �̵�
            rigid.velocity = new Vector2(direction.x * flySpeed, direction.y * flySpeed);
        }
    }
}
