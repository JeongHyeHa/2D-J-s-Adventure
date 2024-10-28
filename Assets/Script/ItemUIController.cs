using TMPro;
using UnityEngine;

public class ItemUIController : MonoBehaviour
{
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI classText;
    public TextMeshProUGUI tryCountText;
    public TextMeshProUGUI scoreText;

    // 데이터를 바인딩하는 함수
    public void SetItemData(int rank, string playerName, string playerClass, string tryCount, string score)
    {
        rankText.text = rank.ToString();
        nameText.text = playerName;
        classText.text = playerClass;
        tryCountText.text = tryCount;
        scoreText.text = score;
    }
}
