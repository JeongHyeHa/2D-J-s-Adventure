using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour
{
    public TextMeshProUGUI uiText;  // Unity의 Text UI 컴포넌트
    public float blinkInterval = 0.7f;  // 깜빡이는 간격(초 단위)
    public Button button;

    void Start()
    {
        if(uiText == null)
        {
            uiText = GameObject.Find("EnterText").GetComponent<TextMeshProUGUI>();
        }
        if (uiText != null)
        {
            StartCoroutine(BlinkText());
        }
        button.interactable = true;
    }

    IEnumerator BlinkText()
    {
        while (true)
        {
            uiText.enabled = !uiText.enabled;  // 텍스트의 활성화 상태를 반전시킴
            yield return new WaitForSeconds(blinkInterval);  // 지정된 시간 동안 대기
        }
    }
}
