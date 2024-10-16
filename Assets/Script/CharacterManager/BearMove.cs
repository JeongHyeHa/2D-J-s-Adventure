using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearMove : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public float moveSpeed = 2f; // ���� �̵� �ӵ�
    private int moveDirection = -1; // �ʱ� ������ -1 (����)

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.Translate(Vector2.right * moveSpeed * moveDirection * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BearGround") && collision.gameObject.name == "mapBearGround")
        {
            moveDirection *= -1;
            spriteRenderer.flipX = moveDirection == -1;
        }
    }
}
