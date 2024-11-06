using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Enemy_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;
    public float speed;
    private int facingDirection = -1;
    private Animator anim;
    private EnemyState enemyState;

    private void Start(){
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    private void Update(){
        if (enemyState == EnemyState.Chasing)
        {
            if(player.position.x > transform.position.x && facingDirection == -1 ||
               player.position.x < transform.position.x && facingDirection == 1){
                Flip();
            }

            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Player")
        {
            if(player == null){
                player = collision.transform;
            }
            ChangeState(EnemyState.Chasing);
        }
    }

     private void OnTriggerExit2D(Collider2D collision){
        if (collision.gameObject.tag == "Player")
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
    }

    private void Flip(){
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    void ChangeState(EnemyState newState){
        //Exit current Animation
        if(enemyState == EnemyState.Idle){
            anim.SetBool("isIdle", false);
        } else if (enemyState == EnemyState.Chasing){
            anim.SetBool("isChasing", false);
        }
        //Update current state
        enemyState = newState;

        //Update new Animation
        if(enemyState == EnemyState.Idle){
            anim.SetBool("isIdle", true);
        } else if (enemyState == EnemyState.Chasing){
            anim.SetBool("isChasing", true);
        }
    }
}

public enum EnemyState{
    Idle,
    Chasing,
}