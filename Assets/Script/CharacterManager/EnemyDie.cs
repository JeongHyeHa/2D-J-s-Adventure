using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxcollider;
    Rigidbody2D rigid;
    public Animator anim;
    AudioSource audioSource;

    public AudioClip monsterDie;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxcollider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    public void OnDamaged()
    {
        anim.SetTrigger("Dead");
        boxcollider.enabled = false;
        audioSource.PlayOneShot(monsterDie);
        
        //Sprite Alpha
        /*spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Sprite Filp Y
        spriteRenderer.flipY = true;
        //Collider Disable
        boxcollider.enabled = false;
        //Die Effect Jump
        rigid.AddForce(Vector2.up*5, ForceMode2D.Impulse);
        rigid.velocity = Vector2.zero;
        //Sound
        audioSource.PlayOneShot(monsterDie);
        //Destroy
        Invoke("DeActive", 5);*/
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
