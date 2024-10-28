using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    public Rigidbody2D rigid;
    public GameObject player;

    public float detectionRange = 6f;      // 플레이어를 추적하는 거리
    public float stopChasingRange = 7f;    // 추적을 멈추는 거리
    public float flySpeed = 2f;            // 날아가는 속도
    
    private bool isChasing = false;        // 추적 상태 플래그
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
            // 매달려 있을 때는 움직이지 않음
            rigid.velocity = Vector2.zero;
        }
    }

    // 플레이어와의 거리를 체크하여 추적 상태를 변경하는 함수
    void CheckPlayerDistance()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(player.transform.position, transform.position);

            // 추적 시작 조건 (플레이어가 가까워질 때)
            if (distance < detectionRange && !isChasing)
            {
                isChasing = true;
                animator.SetBool("flying", true); 
            }
            // 추적 중지 조건 (플레이어가 멀어질 때)
            else if (distance >= stopChasingRange && isChasing)
            {
                isChasing = false;
            }
        }
    }

    // 플레이어를 추적하는 함수
    void ChasePlayer()
    {
        if (player != null)
        {
            // 플레이어를 향한 방향 벡터 계산
            Vector3 direction = (player.transform.position - transform.position).normalized;

            // 방향에 따라 스프라이트를 좌우로 뒤집기
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false;  // 플레이어가 오른쪽에 있을 때
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = true;   // 플레이어가 왼쪽에 있을 때
            }

            // 플레이어 방향으로 이동
            rigid.velocity = new Vector2(direction.x * flySpeed, direction.y * flySpeed);
        }
    }
}
