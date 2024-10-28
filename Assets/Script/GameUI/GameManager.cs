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
    public GameObject rankingPanel;  // ��ŷ �г�
    public Button rankingCloseButton;       // �ݱ� ��ư

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

        // ���� ���� �� �������� ���� �õ� Ƚ�� ��������
        StartCoroutine(networkManager.CoGetPlayerById(int.Parse(LoginManager.Instance.userId), playerData =>
        {
            nowTryCount = playerData.tryCount;  // �������� ������ �õ� Ƚ���� ����
            Debug.Log($"�������� ������ �õ� Ƚ��: {nowTryCount}");
        }));

        // �ݱ� ��ư�� �Լ� ����
        rankingCloseButton.onClick.AddListener(CloseRankingPanel);
    }

    private void Update()
    {
        if (isDead) 
            playerAnim.SetTrigger("doDamaged");

        // ���� �÷��̾ ȭ���� Ŭ���ϸ� ��� ���� �����
        if (Input.GetMouseButtonDown(0) && isRestarting)
        {
            StopCoroutine(RestartCountdown());

            if(nowTryCount >= 3)
            {
                restartText.text = "No more chances!";

                // ��ŷ �г� ����
                //rankingPanel.SetActive(true);
                RankingAnim.SetBool("isPopup", true);
            }
            else
            {
                // �õ�Ƚ�� ������Ʈ
                nowTryCount++;
                StartCoroutine(networkManager.CoPostTryCount(int.Parse(LoginManager.Instance.userId), nowTryCount));
                Debug.Log($"��õ�: {LoginManager.Instance.userId}, {nowTryCount}");

                // ���� �� �ٽ� �ε�
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    // �ݱ� ��ư Ŭ�� �� StartGame ������ ��ȯ
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
            Debug.Log("LoseLife ȣ��!");
            isDead = true;

            //��� ���� ����
            backgroundMusic.Stop();

            if(timer.timeRemaining > 0f)
            {
                //���� ��� ����
                playerLives--;
                heartImages[playerLives].enabled = false;

                //Ÿ�̸� �ð��� ������ ����
                gameScore += (int)Math.Round((120 - int.Parse(timerText.text)) * 0.1, 1);
                UpdateScoreUI();
            }
            
            //�÷��̾� ���� ó��
            player.layer = 11;
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            rigid.velocity = Vector2.zero;

            //���ӿ��� ���� ��ȯ
            gameoverText.gameObject.SetActive(true);
            timerText.gameObject.SetActive(false);
            playerAnim.SetTrigger("OnDamaged");
            backgroundMusic.PlayOneShot(gameOverSound);

            //��õ�â
            StartCoroutine(RestartCountdown());

            // ���� ���� ������ Db�� ����� ���� �����ͺ��� ũ�� DB�� ���� ������ ������Ʈ.
            Debug.Log($"�α��� �� ������ ����: {LoginManager.Instance.userScore}");
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
            // ī��Ʈ�ٿ� ��. ���� ù ȭ������.
            if (i == 0)
            {
                isRestarting = false;
                rankingPanel.SetActive(true);
                RankingAnim.SetBool("isPopup", true);
                yield break;
                /*SceneManager.LoadScene("StartGame");*/
            }

            //�ؽ�Ʈ ������Ʈ
            restartText.text = "Jump To Restart " + i;
            
            //�ؽ�Ʈ �����̱�
            restartText.enabled = false;  
            yield return new WaitForSeconds(0.1f);  
            restartText.enabled = true;  
            yield return new WaitForSeconds(.9f);
        }
    }
}
