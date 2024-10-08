using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameManager : MonoBehaviour
{
    public static StartGameManager Instance { get; private set; }
    public GameObject startCanvas;
    public Image blackScreen;
    public float fadeDuration = 0.5f;       // ���̵� �ƿ��� �ɸ��� �ð�
    public bool isGameStarting = false;    // ������ ���۵Ǿ����� ����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (startCanvas == null)
        {
            startCanvas = GameObject.Find("StartCanvas");
        }
        if (blackScreen == null)
        {
            blackScreen = GameObject.Find("Black").GetComponent<Image>();
        }

        blackScreen.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        // ���콺 Ŭ���� �����Ǹ� ���̵� �ƿ� ����
        if (Input.GetMouseButtonDown(0) && !isGameStarting)
        {
            StartCoroutine(FadeToBlackAndStartGame());
        }
    }

    IEnumerator FadeToBlackAndStartGame()
    {

        float elapsedTime = 0f;

        // ���� ȭ���� ������ ��Ÿ�� (���� -> ������)
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            blackScreen.color = new Color(0, 0, 0, alpha);  // ����(alpha)�� ������Ŵ
            yield return null;
        }

        // ���̵���  �Ϸ� �� ĵ������ ��Ȱ��ȭ�ϰ� ���� ����
        startCanvas.SetActive(false);

        SceneManager.LoadScene("J's Adventure _ver001");
        isGameStarting = true;
    }
}
