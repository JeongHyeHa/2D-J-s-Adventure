using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObjectManager : MonoBehaviour
{
    public List<GameObject> squareObjects;  // 8���� �簢�� ������Ʈ ����Ʈ
    public GameObject triggerPoint;         // �÷��̾ ���� Ʈ���� ����
    public Rigidbody2D rigid;
    public Transform player;
    public AudioSource onTriggerAudio;

    public AudioClip hurtSound;
    private bool trapActivated = false;

    void Start()
    {
        triggerPoint = GameObject.Find("JumpTrigger");
        rigid = GameObject.Find("Player").GetComponent< Rigidbody2D>();
        onTriggerAudio = GetComponent<AudioSource>();

        // ������Ʈ���� ��Ȱ��ȭ ���·� ����
        foreach (GameObject obj in squareObjects)
        {
            obj.SetActive(false);
        }
    }

    // ĳ���Ͱ� Ʈ���ſ� ������ ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !trapActivated) 
        {
            trapActivated = true;
            onTriggerAudio.PlayOneShot(hurtSound);

            foreach (GameObject trap in squareObjects)
            {
                trap.SetActive(true); 
            }
        }
    }
}
