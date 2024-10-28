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
    public GameObject rankingPanel;  // 랭킹 패널
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
        rankingCloseButton.onClick.AddListener(CloseRankingPanel);  // 랭킹 패널 닫기 이벤트
    }

    // 2D 트리거 감지
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGoalin = true;

        // 충돌한 오브젝트가 "Player" 태그를 가지고 있을 때
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Goalin");
            
            BoxCollider2D boxcollider = GetComponent<BoxCollider2D>();
            boxcollider.enabled = false;
        }

        //골인 모드로 전환
        goalText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(false);
        backgroundMusic.Stop();
        backgroundMusic.PlayOneShot(goalinSound);

        //타이머 시간도 점수에 포함
        gameManager.gameScore += (int)Math.Round((120 - int.Parse(timerText.text)) * 0.1, 1);
        gameManager.UpdateScoreUI();

        // 지금 게임 점수가 Db에 저장된 점수 데이터보다 크면 DB의 점수 데이터 업데이트.
        Debug.Log($"로그인 때 가져온 점수: {LoginManager.Instance.userScore}");
        if (gameManager.gameScore > LoginManager.Instance.userScore)
        {
            StartCoroutine(networkManager.CoPostScore(int.Parse(LoginManager.Instance.userId), gameManager.gameScore, () =>
            {
                // 점수 저장 후에 랭킹 패널을 열기
                Invoke("OpenPanelWait3", 3f);
            }));
        }
        else
        {
            Invoke("OpenPanelWait3", 3f);  // 점수가 업데이트되지 않을 때도 패널을 열기
        }
    }

    void OpenPanelWait3()
    {
        rankingManager.StartCoroutine(networkManager.CoGetTop10Scores(rankingManager.DisplayTop10Scores));
        animator.SetBool("isPopup", true);
    }

    // 랭킹 패널 닫기 함수
    private void CloseRankingPanel()
    {
        //rankingPanel.SetActive(false);  // 랭킹 패널을 닫음
        animator.SetBool("isPopup", false);
        SceneManager.LoadScene("StartGame");
    }
}
