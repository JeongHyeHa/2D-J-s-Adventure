using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoTriggerPoint : MonoBehaviour
{
    public List<Rigidbody2D> enemies = new List<Rigidbody2D>();
    public float grv = 4f;   // ����� �߷� ��

    private void Start()
    {
        GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag("Enemy"); // Enemy �±׷� ���� ã��
        foreach (GameObject enemy in foundEnemies)
        {
            if (enemy.name.Contains("dino1"))
            {
                enemies.Add(enemy.GetComponent<Rigidbody2D>());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾ Ʈ���ſ� ����� ���� ����
        if (other.CompareTag("Player"))
        {
            for(int i=0; i< enemies.Count; i++)
            {
                // ������ �߷��� 4�� ����
                foreach (Rigidbody2D enemy in enemies)
                {
                    enemy.gravityScale = grv;
                }
            }
        }
    }
}
