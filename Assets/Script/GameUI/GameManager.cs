using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int gameScore;
    public List<Image> heartImages;  
    public int playerLives = 2;

    public GameObject player;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameoverText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI restartText;
    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer;
    public Animator playerAnim;
    public AudioSource audioSource;
    public AudioSource backgroundMusic;
    NetworkManager networkManager;

    public AudioClip hurtSound;
    public AudioClip gameOverSound;

    public bool isDead = false;
    int nowTryCount;
    bool isRestarting = false;

    private void Awake()
    {
        player = GameObject.Find("Player");
        networkManager = new NetworkManager();

        // ���� ���� �� �������� ���� �õ� Ƚ�� ��������
        StartCoroutine(networkManager.CoGetPlayerById(int.Parse(LoginManager.Instance.userId), playerData =>
        {
            nowTryCount = playerData.tryCount;  // �������� ������ �õ� Ƚ���� ����
            Debug.Log($"�������� ������ �õ� Ƚ��: {nowTryCount}");
        }));
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
                Invoke("ReLoadScene", 3);
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

    void ReLoadScene()
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
            isDead = true;

            //��� ���� ����
            backgroundMusic.Stop();

            //���� ��� ����
            playerLives--;
            heartImages[playerLives].enabled = false;

            //�÷��̾� ���� ó��
            player.layer = 11;
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);

            //Ÿ�̸� �ð��� ������ ����
            gameScore += (int)Math.Round((120-int.Parse(timerText.text)) * 0.1, 1);
            UpdateScoreUI();

            //���ӿ��� ���� ��ȯ
            gameoverText.gameObject.SetActive(true);
            playerAnim.SetTrigger("OnDamaged");
            backgroundMusic.PlayOneShot(gameOverSound);

            //��õ�â
            StartCoroutine(RestartCountdown());
        }
    }

    public void LoseLifeWithPosition(Vector2 targetPos)
    {
        if(playerLives > 1)
        {
            playerLives--;
            heartImages[playerLives].enabled = false;
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

    IEnumerator RestartCountdown()
    {
        isRestarting = true;

        for (int i = 10; i >= 0; i--)
        {
            // ī��Ʈ�ٿ� ��. ���� ù ȭ������.
            if (i == 0)
            {
                isRestarting = false;
                SceneManager.LoadScene("StartGame");
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
