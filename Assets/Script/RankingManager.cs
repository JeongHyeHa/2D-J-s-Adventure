using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform content; // ScrollView의 Content
    public ScrollRect scrollRect; 

    private NetworkManager networkManager;

    void Start()
    {
        networkManager = new NetworkManager();
        StartCoroutine(networkManager.CoGetTop10Scores(DisplayTop10Scores));
    }

    // 상위 10명 데이터를 UI에 표시
    public void DisplayTop10Scores(List<Score> top10Scores)
    {
        if (top10Scores == null || top10Scores.Count == 0)
        {
            Debug.Log("No data available.");
            return;
        }

        foreach (Transform child in content)
        {
            if (child.name != "Guid") 
            {
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < top10Scores.Count; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, content); 
            ItemUIController itemController = newItem.GetComponent<ItemUIController>();

            string tryCount = $"{top10Scores[i].tryCount}/3"; // 시도 횟수
            string score = top10Scores[i].user_score.ToString("N0"); // 점수 포맷

            // 아이템에 데이터 바인딩
            itemController.SetItemData(i + 1, top10Scores[i].user_name, top10Scores[i].department, tryCount, score);
        }

        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void OnRestartGame()
    {
        SceneManager.LoadScene("StartGame");
    }
}
