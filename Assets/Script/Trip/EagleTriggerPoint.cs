using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleTriggerPoint : MonoBehaviour
{
    public EagleMove eagle;

    private void Awake()
    {
        eagle = FindObjectOfType<EagleMove>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾ Ʈ���ſ� ����� �� ������ �̵� ����
        if (other.CompareTag("Player"))
        {
            //eagle.StartMoving(); // ������ �̵� �Լ� ȣ��
        }
    }
}
