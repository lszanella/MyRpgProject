using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;
    public float attackRange = 2;
    public float attackCooldown = 2;
    public float playerDetectRange = 5;
    public Transform detectionPoint;
    public LayerMask playerLayer;

    private float attackCooldownTimer;
    private Rigidbody2D rb;
    private Transform player;
    private int facingDirection = -1;
    private Animator anim;
    private EnemyState enemyState;
    

    private void Start(){
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    private void Update(){

        CheckForPlayer();
        if(attackCooldownTimer > 0){
            attackCooldownTimer -= Time.deltaTime;
        }

        if (enemyState == EnemyState.Chasing)
        {
            Chase();
        } else  if (enemyState == EnemyState.Attacking)
        {
            rb.linearVelocity = Vector2.zero;
        }
        
    }

    void Chase(){
        if(player.position.x > transform.position.x && facingDirection == -1 ||
            player.position.x < transform.position.x && facingDirection == 1){
            Flip();
        }
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    private void CheckForPlayer(){
        Collider2D[] hits = Physics2D.OverlapCircleAll(detectionPoint.position, playerDetectRange, playerLayer);
        if(hits.Length>0){
            player = hits[0].transform;
            //if the player is in the attack range AND cooldown is ready
            if(Vector2.Distance(transform.position, player.position) <= attackRange && attackCooldownTimer <= 0){
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);
            } else if(Vector2.Distance(transform.position, player.position) > attackRange){
                ChangeState(EnemyState.Chasing);
            }
        }else{
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
    }

    private void Flip(){
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void ChangeState(EnemyState newState){
        //Exit current Animation
        if(enemyState == EnemyState.Idle){
            anim.SetBool("isIdle", false);
        } else if (enemyState == EnemyState.Chasing){
            anim.SetBool("isChasing", false);
        } else if (enemyState == EnemyState.Attacking){
            anim.SetBool("isAttacking", false);
        }
        //Update current state
        enemyState = newState;

        //Update new Animation
        if(enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", true);
        
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);
    }
}

public enum EnemyState{
    Idle,
    Chasing,
    Attacking,
}