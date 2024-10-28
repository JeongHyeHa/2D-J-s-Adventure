using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour
{
    public TextMeshProUGUI uiText;  // Unity�� Text UI ������Ʈ
    public float blinkInterval = 0.7f;  // �����̴� ����(�� ����)
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
            uiText.enabled = !uiText.enabled;  // �ؽ�Ʈ�� Ȱ��ȭ ���¸� ������Ŵ
            yield return new WaitForSeconds(blinkInterval);  // ������ �ð� ���� ���
        }
    }
}
