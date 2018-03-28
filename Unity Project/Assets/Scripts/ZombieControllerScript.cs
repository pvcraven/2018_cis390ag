using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieControllerScript : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float timeTravelled = 5f;
    public Transform sightStart, sightEnd;
    public float jumpForce;
	public GameObject player;

    private bool facingLeft = true;
    private bool characterFound = false;
    private bool onGround = false;
    private int jumpCooldown = 120;
    private float flipTime;
    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D cc;
    public float health = 100f;
	private AudioSource audio;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        flipTime = Time.time + timeTravelled;
        anim = GetComponent<Animator>();
		audio = GetComponent<AudioSource> ();
    }

    void FixedUpdate()
    {
		sound_manager ();
        //rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);

        //if (move > 0 && !facingLeft)
        //    Flip();
        //else if (move < 0 && facingLeft)
        //    Flip();

        characterFound = checkForPlayer();
        onGround = DetermineOnGrounded();

        if (jumpCooldown <= 0 && onGround)
        {
            Jump();
            jumpCooldown = 120;
        }
        jumpCooldown--;

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

        if (!onGround && rb.velocity.y > 0)
        {
            anim.SetBool("JumpingUP", true);
        }
        else
        {
            anim.SetBool("JumpingUP", false);
        }
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

    }

	void sound_manager () {
		if (Vector2.Distance (player.transform.position, transform.position) < 5) {
			if (!audio.isPlaying) {
				audio.Play ();
				audio.Play (44100);
			}
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

    public void TakeDamage()
    {
        health -= 50f;

        if (health <= 0)
        {
            Destroy(rb.gameObject);
        }
    }

    bool DetermineOnGrounded()
    {
        int position = 0;
        Collider2D[] overlappingObjects = Physics2D.OverlapCapsuleAll(new Vector2(cc.attachedRigidbody.position.x, cc.attachedRigidbody.position.y), new Vector2(cc.size.x, cc.size.y + .05f), cc.direction, 0);

        //Debug.Log("overlappingObjects: " + overlappingObjects);
        //Debug.Log("Position: " + position);

        while (position < overlappingObjects.GetLength(0))
        {
            if (overlappingObjects[position].CompareTag("Ground"))
            {
                //Debug.Log("On Ground");
                return true;
            }
            position++;
        }
        //Debug.Log("Not on Ground");

        return false;
    }

    public void Jump()
    {
        rb.velocity += Vector2.up * jumpForce;
    }
}
