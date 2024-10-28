using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class StartGameManager : MonoBehaviour
{
    public static StartGameManager Instance { get; private set; }
    
    public GameObject startCanvas;
    public GameObject loginUI;
    public GameObject rankingPanel;
    public Image blackScreen;
    public TextMeshProUGUI startTitleText;
    public TextMeshProUGUI enterText;
    public Button loginCloseButton;
    public Button rankButton;
    public Button enterButton;
    public Button rankingCloseButton;
    public TMP_InputField loginIDInputField;
    public TMP_InputField loginClassInputField;
    public TMP_InputField loginNameInputField;
    public Animator rankAnim;

    private Vector3 targetPosition;         // ��ǥ ��ġ
    private RectTransform startTransform;
    Animator animator;
    Animator loginAnim;
    private float speed = 0.4f;          // �̵� �ӵ�
    public float fadeDuration = 0.5f;       // ���̵� �ƿ��� �ɸ��� �ð�
    public bool isGameStarting = false;     // ������ ���۵Ǿ����� ����

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
        startCanvas = GameObject.Find("StartCanvas");
        loginUI = GameObject.Find("LoginUI");
        blackScreen = GameObject.Find("Black").GetComponent<Image>();
        startTitleText = GameObject.Find("StartTitleText").GetComponent<TextMeshProUGUI>();
        enterText = GameObject.Find("EnterText").GetComponent<TextMeshProUGUI>();
        animator = startTitleText.GetComponent<Animator>();
        loginAnim = loginUI.GetComponent<Animator>();
        loginCloseButton = GameObject.Find("LoginCloseButton").GetComponent<Button>();
        loginIDInputField = GameObject.Find("LoginIDInputField").GetComponent<TMP_InputField>();
        loginClassInputField = GameObject.Find("LoginClassInputField").GetComponent<TMP_InputField>();
        loginNameInputField = GameObject.Find("LoginNameInputField").GetComponent<TMP_InputField>();


        blackScreen.color = new Color(0, 0, 0, 0);
        loginUI.SetActive(false);

        // Ÿ��Ʋ ��ǥ ��ġ
        startTransform = startTitleText.GetComponent<RectTransform>();
        targetPosition = new Vector3(startTransform.anchoredPosition.x, 129f, 0f);
        
        
        loginCloseButton.onClick.AddListener(OnCloseButtonLogin);
        rankButton.onClick.AddListener(OpenRankPanel);
        enterButton.onClick.AddListener(StartGame);
        rankingCloseButton.onClick.AddListener(CloseRankingPanel);
    }

    void Update()
    {
        // ���콺 Ŭ���� �����Ǹ� �α��� ���
        //if (Input.GetMouseButtonDown(0) && !isGameStarting)
        /*if (Input.GetMouseButtonDown(0) && !isGameStarting && !EventSystem.current.IsPointerOverGameObject())

        {
            animator.SetBool("startMove", true);

            // �α��� ȭ������ ��ȯ
            enterText.gameObject.SetActive(false);
            //StartCoroutine(FadeToBlackAndStartGame());
        }*/
    }

    void StartGame()
    {
        animator.SetBool("startMove", true);
        // �α��� ȭ������ ��ȯ
        enterText.gameObject.SetActive(false);
    }

    // �ؽ�Ʈ �ִϸ��̼��� �Ϸ�� �� ȣ��� �Լ� (Animation Event���� ����)
    public void OnTextMoveComplete()
    {
        loginUI.SetActive(true);
        loginAnim.SetBool("isPopup", true);
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

        SceneManager.LoadScene("J's Adventure");
        isGameStarting = true;
    }

    private void OnCloseButtonLogin()
    {
        enterText.gameObject.SetActive(true);
        animator.SetBool("startMove", false);

        loginAnim.SetBool("isPopup", false);
        isGameStarting = false;

        loginIDInputField.text = "";
        loginClassInputField.text = "";
        loginNameInputField.text = "";
    }

    void OpenRankPanel()
    {
        //rankingPanel.SetActive(true);
        rankAnim.SetBool("isPopup", true);
    }

    void CloseRankingPanel()
    {
        rankAnim.SetBool("isPopup", false);
    }
}
