using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoalCollision : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI goalText; 
    public GameObject player;
    public AudioClip goalinSound;
    public Animator animator;
    public GameObject rankingPanel;  // ��ŷ �г�
    public Button rankingCloseButton;

    GameManager gameManager;
    NetworkManager networkManager;
    PlayerController playerController;
    RankingManager rankingManager;

    public bool isGoalin = false;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        networkManager = new NetworkManager();
        playerController = player.GetComponent<PlayerController>();
        rankingManager = rankingPanel.GetComponent<RankingManager>();

        goalText.gameObject.SetActive(false);
        rankingCloseButton.onClick.AddListener(CloseRankingPanel);  // ��ŷ �г� �ݱ� �̺�Ʈ
    }

    // 2D Ʈ���� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGoalin = true;

        // �浹�� ������Ʈ�� "Player" �±׸� ������ ���� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Goalin");
            
            BoxCollider2D boxcollider = GetComponent<BoxCollider2D>();
            boxcollider.enabled = false;
        }

        //���� ���� ��ȯ
        goalText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(false);
        backgroundMusic.Stop();
        backgroundMusic.PlayOneShot(goalinSound);

        //Ÿ�̸� �ð��� ������ ����
        gameManager.gameScore += (int)Math.Round((120 - int.Parse(timerText.text)) * 0.1, 1);
        gameManager.UpdateScoreUI();

        // ���� ���� ������ Db�� ����� ���� �����ͺ��� ũ�� DB�� ���� ������ ������Ʈ.
        Debug.Log($"�α��� �� ������ ����: {LoginManager.Instance.userScore}");
        if (gameManager.gameScore > LoginManager.Instance.userScore)
        {
            StartCoroutine(networkManager.CoPostScore(int.Parse(LoginManager.Instance.userId), gameManager.gameScore, () =>
            {
                // ���� ���� �Ŀ� ��ŷ �г��� ����
                Invoke("OpenPanelWait3", 3f);
            }));
        }
        else
        {
            Invoke("OpenPanelWait3", 3f);  // ������ ������Ʈ���� ���� ���� �г��� ����
        }
    }

    void OpenPanelWait3()
    {
        rankingManager.StartCoroutine(networkManager.CoGetTop10Scores(rankingManager.DisplayTop10Scores));
        animator.SetBool("isPopup", true);
    }

    // ��ŷ �г� �ݱ� �Լ�
    private void CloseRankingPanel()
    {
        //rankingPanel.SetActive(false);  // ��ŷ �г��� ����
        animator.SetBool("isPopup", false);
        SceneManager.LoadScene("StartGame");
    }
}
