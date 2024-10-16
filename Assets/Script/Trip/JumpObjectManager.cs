using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObjectManager : MonoBehaviour
{
    public List<GameObject> squareObjects;  // 8���� �簢�� ������Ʈ ����Ʈ
    public GameObject triggerPoint;         // �÷��̾ ���� Ʈ���� ����
    public Rigidbody2D rigid;
    public Transform player;

    void Start()
    {
        triggerPoint = GameObject.Find("JumpTrigger");
        rigid = GameObject.Find("Player").GetComponent< Rigidbody2D>();

        // ������Ʈ���� ��Ȱ��ȭ ���·� ����
        foreach (GameObject obj in squareObjects)
        {

            obj.SetActive(false);
        }
    }

    void Update()
    {
        // �÷��̾��� ��ġ���� ���� Raycast�� �߻�
        Debug.DrawRay(player.position, Vector2.down * 1.5f, Color.yellow);
        RaycastHit2D hit = Physics2D.Raycast(player.position, Vector2.down, 1.5f, LayerMask.GetMask("Trigger"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.name == "TriggerPoint") 
            {
                Debug.Log("�÷��̾ Ʈ���� ������ �����߽��ϴ�.");
                // ���⼭ �߰� ������ ���� (��: ������Ʈ Ȱ��ȭ)
            }
        }
    }
}
