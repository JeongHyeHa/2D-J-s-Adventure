using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance { get; private set; }

    public TMP_InputField loginIDInputField;
    public TMP_InputField loginClassInputField;
    public TMP_InputField loginNameInputField;
    public TextMeshProUGUI loginMessageText;
    public Button loginButton;
    public AudioSource audioSource;

    public string userId;
    public string department;
    public string userName;
    public int trycount;
    public int userScore;

    NetworkManager networkManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 존재하면 새로 생성되는 객체는 파괴
        }

    }
    void Start()
    {
        networkManager = new NetworkManager();
        loginIDInputField = GameObject.Find("LoginIDInputField").GetComponent<TMP_InputField>();
        loginClassInputField = GameObject.Find("LoginClassInputField").GetComponent<TMP_InputField>();
        loginNameInputField = GameObject.Find("LoginNameInputField").GetComponent<TMP_InputField>();
        loginMessageText = GameObject.Find("LoginMessageText").GetComponent<TextMeshProUGUI>();
        loginButton = GameObject.Find("LoginButton").GetComponent<Button>();
        audioSource = loginButton.GetComponent<AudioSource>();

        loginButton.onClick.AddListener(OnLoginButtonClicked);

        // 각 InputField에 Enter와 Tab 이벤트 연결
        loginIDInputField.onSubmit.AddListener(delegate { OnLoginButtonClicked(); });
        loginClassInputField.onSubmit.AddListener(delegate { OnLoginButtonClicked(); });
        loginNameInputField.onSubmit.AddListener(delegate { OnLoginButtonClicked(); });
    }

    void Update()
    {
        HandleTabAndEnterInput();
    }

    void HandleTabAndEnterInput()
    {
        // Tab 키로 입력 필드 전환 (학과 -> 학번 -> 이름 순으로)
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (loginClassInputField.isFocused) // 학과 -> 학번
            {
                loginIDInputField.Select();
                loginIDInputField.ActivateInputField(); // 커서 깜빡이게 함
            }
            else if (loginIDInputField.isFocused) // 학번 -> 이름
            {
                loginNameInputField.Select();
                loginNameInputField.ActivateInputField();
            }
            else if (loginNameInputField.isFocused) // 이름 -> 학과
            {
                loginClassInputField.Select();
                loginClassInputField.ActivateInputField();
            }
        }

        // Enter 키로 로그인 시도
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnLoginButtonClicked();
        }
    }

    private void OnLoginButtonClicked()
    {
        audioSource.Play(); 

        userId = loginIDInputField.text.Trim();
        department = loginClassInputField.text.Trim().ToLower();
        userName = loginNameInputField.text.Trim();

        if (string.IsNullOrEmpty(userId))
        {
            loginMessageText.text = "ID가 비어있습니다.";
            loginMessageText.color = Color.red;
            return;
        }
        if (string.IsNullOrEmpty(department))
        {
            loginMessageText.text = "학과가 비어있습니다.";
            loginMessageText.color = Color.red;
            return;
        }
        if (string.IsNullOrEmpty(userName))
        {
            loginMessageText.text = "이름이 비어있습니다.";
            loginMessageText.color = Color.red;
            return;
        }

        loginMessageText.text = "정보를 조회 중입니다...";
        loginMessageText.color = Color.green;

        // ID로 로그인 시도
        StartCoroutine(networkManager.CoGetPlayerById(int.Parse(userId), user =>
        {    
            if (user == null)
            {
                loginMessageText.text = "환영합니다! 회원가입을 시작합니다.";

                Score newPlayer = new Score
                {
                    user_id = int.Parse(userId),
                    department = department,
                    user_name = userName,
                    tryCount = 0,
                    user_score = 0
                };

                // 회원가입 요청
                StartCoroutine(networkManager.CoPostPlayer(newPlayer, () =>
                {
                    loginMessageText.text = "회원가입이 완료되었습니다. 게임을 시작합니다.";
                    loginMessageText.color = Color.green;

                    // 새로운 사용자 데이터 저장
                    Instance.userId = newPlayer.user_id.ToString();
                    Instance.department = newPlayer.department;
                    Instance.userName = newPlayer.user_name;
                    Instance.trycount = newPlayer.tryCount;
                    Instance.userScore = newPlayer.user_score;
                    Debug.Log($"회원가입: {userId}, {department}, {userName}, {trycount}, {userScore}");

                    // 시도횟수 +1
                    StartCoroutine(networkManager.CoPostTryCount(int.Parse(userId), ++trycount));

                    SceneManager.LoadScene("J's Adventure");
                }));
            }
            else
            {
                Debug.Log($"로그인: {user.user_id}, {department}, {user.user_name}, {user.tryCount}, {user.user_score}");
                // 사용자가 존재할 경우 로그인 성공
                if (user.user_name != userName || user.department != department)
                {
                    loginMessageText.text = "이름 또는 학과를 잘못 입력하셨습니다.";
                    loginMessageText.color = Color.red;
                    return;
                }
                else if (user.tryCount >= 3)
                {
                    loginMessageText.text = "로그인 시도 횟수 초과. 더 이상 로그인할 수 없습니다.";
                    loginMessageText.color = Color.red;
                    return;
                }
                else
                {
                    loginMessageText.text = "로그인 성공! 게임을 시작합니다.";

                    // 새로운 사용자 데이터 저장
                    Instance.userId = user.user_id.ToString();
                    Instance.department = user.department;
                    Instance.userName = user.user_name;
                    Instance.trycount = user.tryCount;
                    Instance.userScore = user.user_score;

                    // 시도횟수 +1
                    StartCoroutine(networkManager.CoPostTryCount(int.Parse(userId), ++trycount));

                    SceneManager.LoadScene("J's Adventure");
                }
            }
        }));
    }
}