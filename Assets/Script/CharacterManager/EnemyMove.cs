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
    public float detectionRange = 10;    // 플레이어를 추적하는 거리
    public float stopChasingRange = 6f;    // 추적을 멈추는 거리
    public bool isChasing = false;      // 추적 상태 관리

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        gameManager = FindObjectOfType<GameManager>();

        // 처음에는 랜덤으로 움직임
        Invoke("Think", 5f);
    }

    void FixedUpdate()
    {
        if (gameManager != null && gameManager.isDead)
        {
            if (!IsInvoking("Think"))
            {
                Invoke("Think", 5f);  // 랜덤 이동을 주기적으로 수행
            }

            return;  // 더 이상 추적하지 않고 여기서 종료
        }

        // 추적 중이 아니라면 랜덤으로 움직임
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
            // 추적 중이면 플레이어 방향으로 움직임
            Chase();
        }

        // 플레이어와의 거리 측정
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

            // 추적 시작 조건 (플레이어가 가까울 때)
            if (distance < detectionRange && !isChasing)
            {
                isChasing = true;
                CancelInvoke("Think"); 
            }
            // 추적 중지 조건 (플레이어가 멀어졌을 때)
            else if (distance >= stopChasingRange && isChasing)
            {
                isChasing = false;
                Invoke("Think", 5f); // 다시 랜덤으로 걷기 시작
            }
        }
    }

    void Chase()
    {
        if(player != null)
        {
            // 플레이어를 향한 방향 벡터 계산
            Vector3 direction = player.transform.position - transform.position;

            // 플레이어가 오른쪽에 있으면 오른쪽으로, 왼쪽에 있으면 왼쪽으로 추적
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

            // 추적 중일 때는 빠르게 이동
            rigid.velocity = new Vector2(nextmove * 2f, rigid.velocity.y); // 2배 빠르게 추적
        }
    }
}
