using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameoverText;
    GameManager gameManager;
    GoalCollision goalCollision;

    public float timeRemaining = 120f; // 120�� (2��)
    bool isCountdownEnded = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        goalCollision = FindObjectOfType<GoalCollision>();

        // �ʱ� �ؽ�Ʈ ����
        timerText.text = Mathf.RoundToInt(timeRemaining).ToString();

        gameoverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (timeRemaining > 0 && !gameManager.isDead || timeRemaining > 0 &&  !goalCollision.isGoalin)
        {
            timeRemaining -= Time.deltaTime;

            // ���� �ð��� ��Ȯ�� 10�� ������ �� �ؽ�Ʈ ������ ���������� ����
            if (Mathf.RoundToInt(timeRemaining) <= 10)
            {
                timerText.color = Color.red;
            }

            // �� ������ �ؽ�Ʈ ������Ʈ (�ݿø��� �� ǥ��)
            int displayTime = Mathf.RoundToInt(timeRemaining);
            if (displayTime > 0)
            {
                timerText.text = displayTime.ToString();
            }
        }
        else if(!isCountdownEnded && timeRemaining <= 0)
        {
            isCountdownEnded = true;
            // �ð��� �� �Ǿ��� �� ó��
            timeRemaining = 0;
            timerText.text = "0";
            gameManager.isDead = true;
            gameManager.LoseLife();
        }
    }
}
