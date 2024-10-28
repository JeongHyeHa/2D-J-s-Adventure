using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int gameScore=0;
    public List<Image> heartImages;  
    public int playerLives = 2;

    public GameObject player;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameoverText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI restartText;
    public GameObject rankingPanel;  // 랭킹 패널
    public Button rankingCloseButton;       // 닫기 버튼

    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer;
    public Animator playerAnim;
    public Animator RankingAnim;
    public AudioSource audioSource;
    public AudioSource backgroundMusic;
    public AudioClip hurtSound;
    public AudioClip gameOverSound;

    NetworkManager networkManager;
    CountdownTimer timer;

    public bool isDead = false;
    int nowTryCount;
    bool isRestarting = false;

    private void Awake()
    {
        player = GameObject.Find("Player");
        networkManager = new NetworkManager();
        timer = FindObjectOfType<CountdownTimer>();

        // 게임 시작 시 서버에서 현재 시도 횟수 가져오기
        StartCoroutine(networkManager.CoGetPlayerById(int.Parse(LoginManager.Instance.userId), playerData =>
        {
            nowTryCount = playerData.tryCount;  // 서버에서 가져온 시도 횟수를 저장
            Debug.Log($"서버에서 가져온 시도 횟수: {nowTryCount}");
        }));

        // 닫기 버튼에 함수 연결
        rankingCloseButton.onClick.AddListener(CloseRankingPanel);
    }

    private void Update()
    {
        if (isDead) 
            playerAnim.SetTrigger("doDamaged");

        // 만약 플레이어가 화면을 클릭하면 즉시 게임 재시작
        if (Input.GetMouseButtonDown(0) && isRestarting)
        {
            StopCoroutine(RestartCountdown());

            if(nowTryCount >= 3)
            {
                restartText.text = "No more chances!";

                // 랭킹 패널 띄우기
                //rankingPanel.SetActive(true);
                RankingAnim.SetBool("isPopup", true);
            }
            else
            {
                // 시도횟수 업데이트
                nowTryCount++;
                StartCoroutine(networkManager.CoPostTryCount(int.Parse(LoginManager.Instance.userId), nowTryCount));
                Debug.Log($"재시도: {LoginManager.Instance.userId}, {nowTryCount}");

                // 현재 씬 다시 로드
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    // 닫기 버튼 클릭 시 StartGame 씬으로 전환
    public void CloseRankingPanel()
    {
        RankingAnim.SetBool("isPopup", false);
        Invoke("OnRestartGame", 2f);
    }

    public void OnRestartGame()
    {
        SceneManager.LoadScene("StartGame");
    }

    public void UpdateScoreUI()
    {
        scoreText.text = $"Score : {gameScore}";
    }

    public void LoseLife()
    {
        if(playerLives <=1 || isDead)
        {
            Debug.Log("LoseLife 호출!");
            isDead = true;

            //배경 음악 멈춤
            backgroundMusic.Stop();

            if(timer.timeRemaining > 0f)
            {
                //남은 목숨 삭제
                playerLives--;
                heartImages[playerLives].enabled = false;

                //타이머 시간도 점수에 포함
                gameScore += (int)Math.Round((120 - int.Parse(timerText.text)) * 0.1, 1);
                UpdateScoreUI();
            }
            
            //플레이어 죽음 처리
            player.layer = 11;
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            rigid.velocity = Vector2.zero;

            //게임오버 모드로 전환
            gameoverText.gameObject.SetActive(true);
            timerText.gameObject.SetActive(false);
            playerAnim.SetTrigger("OnDamaged");
            backgroundMusic.PlayOneShot(gameOverSound);

            //재시도창
            StartCoroutine(RestartCountdown());

            // 지금 게임 점수가 Db에 저장된 점수 데이터보다 크면 DB의 점수 데이터 업데이트.
            Debug.Log($"로그인 때 가져온 점수: {LoginManager.Instance.userScore}");
            if (gameScore > LoginManager.Instance.userScore)
            {
                StartCoroutine(networkManager.CoPostScore(int.Parse(LoginManager.Instance.userId), gameScore, () =>
                {
                    return;
                }));
            }
        }
    }

    public void LoseLifeWithPosition(Vector2 targetPos)
    {
        if(playerLives > 1)
        {
            playerLives--;
            heartImages[playerLives].enabled = false;
            gameScore -= 5;
            UpdateScoreUI();
            OnDamaged(targetPos);
        }
    }

    public void OnDamaged(Vector2 targetPos)
    {
        //Change Layer(Immortal Active)
        player.layer = 11;
        //View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);
        //Animation
        playerAnim.SetTrigger("doDamaged");
        //Sound
        audioSource.PlayOneShot(hurtSound);
        Invoke("OffDamaged", 3);
    }

    void OffDamaged()
    {
        player.layer = 9;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public IEnumerator RestartCountdown()
    {
        isRestarting = true;

        for (int i = 10; i >= 0; i--)
        {
            // 카운트다운 끝. 게임 첫 화면으로.
            if (i == 0)
            {
                isRestarting = false;
                rankingPanel.SetActive(true);
                RankingAnim.SetBool("isPopup", true);
                yield break;
                /*SceneManager.LoadScene("StartGame");*/
            }

            //텍스트 업데이트
            restartText.text = "Jump To Restart " + i;
            
            //텍스트 깜빡이기
            restartText.enabled = false;  
            yield return new WaitForSeconds(0.1f);  
            restartText.enabled = true;  
            yield return new WaitForSeconds(.9f);
        }
    }
}
