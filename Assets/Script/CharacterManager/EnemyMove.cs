using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Rigidbody2D rigid;
    public SpriteRenderer sprite;
    public GameObject player;
    public GameManager gameManager;

    public int nextmove;
    public float detectionRange = 10;    // �÷��̾ �����ϴ� �Ÿ�
    public float stopChasingRange = 6f;    // ������ ���ߴ� �Ÿ�
    public bool isChasing = false;      // ���� ���� ����

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        gameManager = FindObjectOfType<GameManager>();

        // ó������ �������� ������
        Invoke("Think", 5f);
    }

    void FixedUpdate()
    {
        if (gameManager != null && gameManager.isDead)
        {
            if (!IsInvoking("Think"))
            {
                Invoke("Think", 5f);  // ���� �̵��� �ֱ������� ����
            }

            return;  // �� �̻� �������� �ʰ� ���⼭ ����
        }

        // ���� ���� �ƴ϶�� �������� ������
        if (!isChasing)
        {
            //Move
            rigid.velocity = new Vector2(nextmove * 1.5f, rigid.velocity.y);

            //Platform Check
            Vector2 frontVec = new Vector2(rigid.position.x + nextmove * 0.3f, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
            if (rayHit.collider == null)
                Turn();
        }
        else
        {
            // ���� ���̸� �÷��̾� �������� ������
            Chase();
        }

        // �÷��̾���� �Ÿ� ����
        CheckPlayerDistance();
    }

    void Think()
    {
        if (!isChasing)
        {
            //Set next active
            nextmove = Random.Range(-1, 2);

            //Filp Sprite
            if (nextmove != 0)
                sprite.flipX = nextmove == 1;

            //Recursive
            Invoke("Think", 5f);
        }
    }

    void Turn()
    {
        nextmove *= -1;
        sprite.flipX = nextmove == 1;

        CancelInvoke();
        Invoke("Think", 2);
    }

    void CheckPlayerDistance()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(player.transform.position, transform.position);

            // ���� ���� ���� (�÷��̾ ����� ��)
            if (distance < detectionRange && !isChasing)
            {
                isChasing = true;
                CancelInvoke("Think"); 
            }
            // ���� ���� ���� (�÷��̾ �־����� ��)
            else if (distance >= stopChasingRange && isChasing)
            {
                isChasing = false;
                Invoke("Think", 5f); // �ٽ� �������� �ȱ� ����
            }
        }
    }

    void Chase()
    {
        if(player != null)
        {
            // �÷��̾ ���� ���� ���� ���
            Vector3 direction = player.transform.position - transform.position;

            // �÷��̾ �����ʿ� ������ ����������, ���ʿ� ������ �������� ����
            if (direction.x > 0)
            {
                nextmove = 1;
                sprite.flipX = true;
            }
            else
            {
                nextmove = -1;
                sprite.flipX = false;
            }

            // ���� ���� ���� ������ �̵�
            rigid.velocity = new Vector2(nextmove * 2f, rigid.velocity.y); // 2�� ������ ����
        }
    }
}
