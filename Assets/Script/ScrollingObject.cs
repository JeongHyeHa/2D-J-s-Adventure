using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    public float speed = 10f;   // ��ũ�� �̵� �ӵ�

     void Update()
    {
        // �ʴ� speed�� �ӵ��� ���������� ���� �̵�
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
