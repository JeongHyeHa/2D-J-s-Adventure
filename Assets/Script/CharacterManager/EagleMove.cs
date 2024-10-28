using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EagleMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRender;

    public int nextMove = -1;
    public float moveSpeed = 3f;  
    /*public float minX = 99.89f;  
    public float maxX = 120.89f;*/
    public float eaglePosX;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRender = GetComponent<SpriteRenderer>();
        eaglePosX = GetComponent<Transform>().position.x;
        /*Invoke("Think", 1.7f);*/
    }

    void FixedUpdate()
    {
        //Move
        transform.Translate(Vector2.right * moveSpeed * nextMove * Time.deltaTime);
        //rigid.velocity = new Vector2(moveSpeed * nextMove, rigid.velocity.y);

        if (transform.position.x < eaglePosX-5.3f || transform.position.x > eaglePosX+3f)
        {
            nextMove *= -1;  
            spriteRender.flipX = nextMove == 1; 
        }
    }

    void Think()
    {
        nextMove *=-1;
        spriteRender.flipX = nextMove == 1;
        Invoke("Think", 2f);
    }
}
