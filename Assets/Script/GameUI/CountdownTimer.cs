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

    public float timeRemaining = 120f; // 120초 (2분)
    bool isCountdownEnded = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        goalCollision = FindObjectOfType<GoalCollision>();

        // 초기 텍스트 설정
        timerText.text = Mathf.RoundToInt(timeRemaining).ToString();

        gameoverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (timeRemaining > 0 && !gameManager.isDead || timeRemaining > 0 &&  !goalCollision.isGoalin)
        {
            timeRemaining -= Time.deltaTime;

            // 남은 시간이 정확히 10초 이하일 때 텍스트 색상을 빨간색으로 변경
            if (Mathf.RoundToInt(timeRemaining) <= 10)
            {
                timerText.color = Color.red;
            }

            // 초 단위로 텍스트 업데이트 (반올림된 값 표시)
            int displayTime = Mathf.RoundToInt(timeRemaining);
            if (displayTime > 0)
            {
                timerText.text = displayTime.ToString();
            }
        }
        else if(!isCountdownEnded && timeRemaining <= 0)
        {
            isCountdownEnded = true;
            // 시간이 다 되었을 때 처리
            timeRemaining = 0;
            timerText.text = "0";
            gameManager.isDead = true;
            gameManager.LoseLife();
        }
    }
}
