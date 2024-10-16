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

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
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
                
                if(CheckIfCanHang())
                    animator.SetBool("flying", false);
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

            // �÷��̾� �������� �̵�
            rigid.velocity = new Vector2(direction.x * flySpeed, direction.y * flySpeed);
        }
    }

    private bool CheckIfCanHang()
    {
        // ���� �Ӹ� ���� Raycast�� ��Ƽ� ���� �ִ� ������ Ȯ��
        Vector2 rayOrigin = new Vector2(rigid.position.x, rigid.position.y);
        RaycastHit2D rayHit = Physics2D.Raycast(rayOrigin, Vector2.up, 1.0f, LayerMask.GetMask("Ground"));

        Debug.DrawRay(rayOrigin, Vector2.up * 1.0f, Color.green); // �ð������� Ray�� �׸���

        // ���� "Ground" ���̾�� �浹�ߴٸ� true ��ȯ (�Ŵ޸� �� ����)
        if (rayHit.collider != null)
        {
            return true;
        }

        // �Ŵ޸� ������ ������ false ��ȯ
        return false;
    }
}
