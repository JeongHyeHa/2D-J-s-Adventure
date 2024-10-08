using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;    // ī�޶� ����ٴ� ���
    public float smooth = 0.1f; // ī�޶� �����̴� �ӵ�
    public Vector3 adjustCamPos;    // ī�޶� ��ġ ����
    public Vector2 minCamLimit; // ī�޶� ��� ����
    public Vector2 maxCamLimit;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        //adjustCamPos = new Vector3(0, 0.3f, 0);
        //minCamLimit = new Vector2(2.594f, -8.5f);
        //maxCamLimit = new Vector2(183.6f, 1.92f);
    }

    void Update()
    {
        if (target == null) return;

        // ī�޶��� ��� ��ġ ���� ����
        Vector3 pos = Vector3.Lerp(transform.position, target.position, smooth);

        // ���� �Ѱ� ��ġ�� ���� ī�޶� ��ġ
        transform.position = new Vector3(
            Mathf.Clamp(pos.x, minCamLimit.x, maxCamLimit.x) + adjustCamPos.x,
            Mathf.Clamp(pos.y, minCamLimit.y, maxCamLimit.y) + adjustCamPos.y,
            -10f + adjustCamPos.z);
    }
}
