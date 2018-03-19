using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieControllerScript : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float timeTravelled = 5f;
    public Transform raycastSpawn;

    private bool facingLeft = false;
    private bool characterFound = false;
    private float flipTime;
    private Rigidbody2D rb;
    //private Animator anim;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        flipTime = Time.time + timeTravelled;
        //anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //anim.SetFloat("vSpeed", rb.velocity.y);

        characterFound = checkForPlayer();

        if(!characterFound)
        {
            if(Time.time < flipTime)
            {
                if(facingLeft)
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
        //if(facingLeft)
        //{
        //    if(Physics2D.Raycast(rb.transform.position, Vector3.back, 1, 13))
        //    {
        //        return true;
        //    }
        //}
        //else
        //{
        //    if(Physics2D.Raycast(rb.transform.position,Vector3.forward, 1, 13))
        //    {
        //        return true;
        //    }
        //}

        Debug.Log(Physics2D.Raycast(raycastSpawn.position, Vector3.forward, 1, 13).collider.gameObject.tag);

        return false;
    }
}
