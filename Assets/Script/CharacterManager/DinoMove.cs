using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoMove : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // �߷��� ����Ǹ� �̵�
        if (rb.gravityScale > 0)
        {
            // �̵� �ӵ��� �� ĭ�� �������� ����
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
    }
}
