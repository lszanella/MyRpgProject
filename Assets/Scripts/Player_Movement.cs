using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public int speed;
    public Rigidbody2D rb;
    public Animator anim;
    private int facingDirection = 1;

    // FixUpdate is called 50x frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal > 0 && transform.localScale.x < 0 ||
            horizontal < 0 && transform.localScale.x > 0)
        {
            Flip();
        }

        anim.SetFloat("horizontal", Mathf.Abs(horizontal));
        anim.SetFloat("vertical", Mathf.Abs(vertical));
        
        rb.linearVelocity = new Vector2(horizontal, vertical)*speed;
    }

    private void Flip(){
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
