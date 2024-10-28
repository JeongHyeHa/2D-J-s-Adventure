using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObjectManager : MonoBehaviour
{
    public List<GameObject> squareObjects;  // 8개의 사각형 오브젝트 리스트
    public GameObject triggerPoint;         // 플레이어가 밟을 트리거 지점
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

        // 오브젝트들을 비활성화 상태로 시작
        foreach (GameObject obj in squareObjects)
        {
            obj.SetActive(false);
        }
    }

    // 캐릭터가 트리거에 들어왔을 때
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
