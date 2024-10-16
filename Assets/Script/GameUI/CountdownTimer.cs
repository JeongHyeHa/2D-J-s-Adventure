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

    private float timeRemaining = 120f; // 120�� (2��)

    void Start()
    {
        timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        gameoverText = GameObject.Find("GameoverText").GetComponent<TextMeshProUGUI>();
        gameManager = FindObjectOfType<GameManager>();

        // �ʱ� �ؽ�Ʈ ����
        timerText.text = Mathf.RoundToInt(timeRemaining).ToString();

        gameoverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (timeRemaining > 0 && !gameManager.isDead)
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
        else 
        {
            // �ð��� �� �Ǿ��� �� ó��
            timeRemaining = 0;
            timerText.text = "0";
            gameManager.isDead = true;
            OnCountdownEnd();
        }
    }

    // ī��Ʈ�ٿ��� ������ �� ����� ����
    void OnCountdownEnd()
    {
        Debug.Log("Countdown has ended!");
        timerText.text = "";
        gameManager.isDead = true;
        // �߰����� ó�� ���� (��: ���� ����, �̺�Ʈ Ʈ���� ��)
    }
}
