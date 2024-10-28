using UnityEngine;

public class AlternatingTerrainMover : MonoBehaviour
{
    public Transform[] legs;
    public float moveSpeed = 2f; 
    public float moveDistance = 2f; 
    public float phaseDifference = Mathf.PI / 2; 

    private Vector3[] startPositions; 

    void Start()
    {
        startPositions = new Vector3[legs.Length];
        for (int i = 0; i < legs.Length; i++)
        {
            startPositions[i] = legs[i].position;
        }
    }

    void Update()
    {
        for (int i = 0; i < legs.Length; i++)
        {
            // �� �ٸ��� ���� ���̸� �༭ �����ϴ� ȿ�� �����
            float newY = Mathf.Sin(Time.time * moveSpeed + i * phaseDifference) * moveDistance;
            legs[i].position = new Vector3(startPositions[i].x, startPositions[i].y + newY, startPositions[i].z);
        }
    }
}
