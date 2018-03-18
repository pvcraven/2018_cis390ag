using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieControllerScript : MonoBehaviour
{
    public float maxSpeed = 10f;

    private bool facingLeft = true;
    private Rigidbody2D rb;
    private Animator anim;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        anim.SetFloat("vSpeed", rb.velocity.y);

        var move = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(move));
        rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);

        if (move > 0 && !facingLeft)
        {
            Flip();
        }
        else if (move < 0 && facingLeft)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        var theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
