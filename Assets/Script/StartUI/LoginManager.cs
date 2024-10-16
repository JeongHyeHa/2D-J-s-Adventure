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
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �����ϸ� ���� �����Ǵ� ��ü�� �ı�
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

        // �� InputField�� Enter�� Tab �̺�Ʈ ����
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
        // Tab Ű�� �Է� �ʵ� ��ȯ (�а� -> �й� -> �̸� ������)
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (loginClassInputField.isFocused) // �а� -> �й�
            {
                loginIDInputField.Select();
                loginIDInputField.ActivateInputField(); // Ŀ�� �����̰� ��
            }
            else if (loginIDInputField.isFocused) // �й� -> �̸�
            {
                loginNameInputField.Select();
                loginNameInputField.ActivateInputField();
            }
            else if (loginNameInputField.isFocused) // �̸� -> �а�
            {
                loginClassInputField.Select();
                loginClassInputField.ActivateInputField();
            }
        }

        // Enter Ű�� �α��� �õ�
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnLoginButtonClicked();
        }
    }

    private void OnLoginButtonClicked()
    {
        audioSource.Play(); 

        userId = loginIDInputField.text.Trim();
        department = loginClassInputField.text.Trim();
        userName = loginNameInputField.text.Trim();

        if (string.IsNullOrEmpty(userId))
        {
            loginMessageText.text = "ID�� ����ֽ��ϴ�.";
            loginMessageText.color = Color.red;
            return;
        }
        if (string.IsNullOrEmpty(department))
        {
            loginMessageText.text = "�а��� ����ֽ��ϴ�.";
            loginMessageText.color = Color.red;
            return;
        }
        if (string.IsNullOrEmpty(userName))
        {
            loginMessageText.text = "�̸��� ����ֽ��ϴ�.";
            loginMessageText.color = Color.red;
            return;
        }

        loginMessageText.text = "������ ��ȸ ���Դϴ�...";
        loginMessageText.color = Color.green;

        // ID�� �α��� �õ�
        StartCoroutine(networkManager.CoGetPlayerById(int.Parse(userId), user =>
        {    
            if (user == null)
            {
                loginMessageText.text = "ȯ���մϴ�! ȸ�������� �����մϴ�.";

                Score newPlayer = new Score
                {
                    user_id = int.Parse(userId),
                    department = department,
                    user_name = userName,
                    tryCount = 0,
                    user_score = 0
                };

                // ȸ������ ��û
                StartCoroutine(networkManager.CoPostPlayer(newPlayer, () =>
                {
                    loginMessageText.text = "ȸ�������� �Ϸ�Ǿ����ϴ�. ������ �����մϴ�.";
                    loginMessageText.color = Color.green;

                    // ���ο� ����� ������ ����
                    Instance.userId = newPlayer.user_id.ToString();
                    Instance.department = newPlayer.department;
                    Instance.userName = newPlayer.user_name;
                    Instance.trycount = newPlayer.tryCount;
                    Instance.userScore = newPlayer.user_score;
                    Debug.Log($"ȸ������: {userId}, {department}, {userName}, {trycount}, {userScore}");
                    
                    SceneManager.LoadScene("J's Adventure");
                }));
            }
            else
            {
                Debug.Log($"�α���: {user.user_id}, {department}, {user.user_name}, {user.tryCount}, {user.user_score}");
                // ����ڰ� ������ ��� �α��� ����
                if (user.user_name != userName || user.department != department)
                {
                    loginMessageText.text = "�̸� �Ǵ� �а��� �߸� �Է��ϼ̽��ϴ�.";
                    loginMessageText.color = Color.red;
                    return;
                }
                else if (user.tryCount >= 3)
                {
                    loginMessageText.text = "�α��� �õ� Ƚ�� �ʰ�. �� �̻� �α����� �� �����ϴ�.";
                    loginMessageText.color = Color.red;
                    return;
                }
                else
                {
                    loginMessageText.text = "�α��� ����! ������ �����մϴ�.";

                    // ���ο� ����� ������ ����
                    Instance.userId = user.user_id.ToString();
                    Instance.department = user.department;
                    Instance.userName = user.user_name;
                    Instance.trycount = user.tryCount;
                    Instance.userScore = user.user_score;

                    // �õ�Ƚ�� +1
                    StartCoroutine(networkManager.CoPostTryCount(int.Parse(userId), ++trycount));

                    SceneManager.LoadScene("J's Adventure");
                }
            }
        }));
    }
}