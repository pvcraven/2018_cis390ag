using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieControllerScript : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float timeTravelled = 5f;
    public Transform sightStart, sightEnd;

    private bool facingLeft = true;
    private bool characterFound = false;
    private float flipTime;
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        flipTime = Time.time + timeTravelled;
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        var move = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(move));
        //rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);

        if (move > 0 && !facingLeft)
            Flip();
        else if (move < 0 && facingLeft)
            Flip();

        characterFound = checkForPlayer();

        if (!characterFound)
        {
            if (Time.time < flipTime)
            {
                if (facingLeft)
                {
                    rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
                }
            }
            else
            {
                Flip();
                flipTime = Time.time + timeTravelled;
            }
        }
        else
        {
            Debug.Log("Player found!");
            //Code for movement following player after player has been found
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        var theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    bool checkForPlayer()
    {
        return Physics2D.Linecast(sightStart.position, sightEnd.position, 1 << LayerMask.NameToLayer("Player"));
    }
}
