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

    private Vector3 targetPosition;         // 목표 위치
    private RectTransform startTransform;
    Animator animator;
    Animator loginAnim;
    private float speed = 0.4f;          // 이동 속도
    public float fadeDuration = 0.5f;       // 페이드 아웃에 걸리는 시간
    public bool isGameStarting = false;     // 게임이 시작되었는지 여부

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

        // 타이틀 목표 위치
        startTransform = startTitleText.GetComponent<RectTransform>();
        targetPosition = new Vector3(startTransform.anchoredPosition.x, 129f, 0f);
        
        
        loginCloseButton.onClick.AddListener(OnCloseButtonLogin);
        rankButton.onClick.AddListener(OpenRankPanel);
        enterButton.onClick.AddListener(StartGame);
        rankingCloseButton.onClick.AddListener(CloseRankingPanel);
    }

    void Update()
    {
        // 마우스 클릭이 감지되면 로그인 모드
        //if (Input.GetMouseButtonDown(0) && !isGameStarting)
        /*if (Input.GetMouseButtonDown(0) && !isGameStarting && !EventSystem.current.IsPointerOverGameObject())

        {
            animator.SetBool("startMove", true);

            // 로그인 화면으로 전환
            enterText.gameObject.SetActive(false);
            //StartCoroutine(FadeToBlackAndStartGame());
        }*/
    }

    void StartGame()
    {
        animator.SetBool("startMove", true);
        // 로그인 화면으로 전환
        enterText.gameObject.SetActive(false);
    }

    // 텍스트 애니메이션이 완료된 후 호출될 함수 (Animation Event에서 설정)
    public void OnTextMoveComplete()
    {
        loginUI.SetActive(true);
        loginAnim.SetBool("isPopup", true);
    }

    IEnumerator FadeToBlackAndStartGame()
    {

        float elapsedTime = 0f;

        // 검은 화면이 서서히 나타남 (투명 -> 불투명)
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            blackScreen.color = new Color(0, 0, 0, alpha);  // 투명도(alpha)를 증가시킴
            yield return null;
        }

        // 페이드인  완료 후 캔버스를 비활성화하고 게임 시작
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
