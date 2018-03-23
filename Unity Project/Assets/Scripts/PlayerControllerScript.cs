using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
	#region Attributes

	public float maxSpeed = 5f;
	public float jumpForce = 5f;
	public float groundDistance;
	public float fallMultiplier = 2f;
	public float lowJumpMultiplier = 2f;
	public GameObject bullet;
	public Transform bulletSpawn;
	public float fireRate = 1f;
    private SpriteRenderer sr;

    // If the character begins the level facing left, this needs to be changed to false.
    private bool facingRight = true;
	private bool isGrounded = false;

    public Transform startOnPlayer, endOnGround;

    //Anim Variables
    private bool stabbing = false;
    private bool walking = false;
    private float vSpeed = 0f;

	private bool hasWeapon = true;
	private float nextFire;
	private List<Collision2D> groundPlayerIsTouching = new List<Collision2D>();
	private RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
	private List<RaycastHit2D> hitBufferList = new List<RaycastHit2D> (16);
	private ContactFilter2D contactFilter;

	#endregion

	#region Components

	GameObject[] food;
	GameObject[] weapons;
	GameObject[] items;
	GameObject[] enemies;
	Animator anim;
	private Rigidbody2D rb2D;
	private Collider2D coll2D;

	#endregion

	void Start()
	{

		anim = GetComponent<Animator>();
		rb2D = GetComponent<Rigidbody2D>();
		coll2D = GetComponent<Collider2D>();

		food = GameObject.FindGameObjectsWithTag("Food");
		weapons = GameObject.FindGameObjectsWithTag("Weapon");
		items = GameObject.FindGameObjectsWithTag("Item");
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
	}

	// Use when applying non-physics-related functions. Runs once per frame.
	void Update()
	{
		CheckForInput();
		ApplyFallMultipliers();
		CheckIfTouchingItems();
		CheckIfTouchingEnemy();
        CheckIfGrounded();
	}

// Use when applying physics-related functions. Runs in sync with the physics engine - may update 0, 1, or many times per frame depending on the physics FPS settings.
	void FixedUpdate()
	{
		CheckForInput();
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(FlashColor());
        }
    }
    
    #region Logic Functions

    private void CheckForInput()
	{
		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			Jump();
		}

		if (Input.GetButton("Shoot") && hasWeapon && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			FireWeapon();
		}

        stabbing = Input.GetKeyDown("f");

        if (stabbing)
        {
            Stab();
        }

        MoveHorizontally();
	}

	private void CheckIfTouchingItems()
	{
		foreach (GameObject item in food)
		{
			if (item.GetComponent<Collider2D>().IsTouching(coll2D))
			{
				Debug.Log("Colliding with item");
				if (Input.GetButton("Interact"))
				{
					Destroy(item);
				}
			}
		}

		foreach (GameObject item in weapons)
		{
			if (item.GetComponent<Collider2D>().IsTouching(coll2D))
			{
				if (Input.GetButton("Interact"))
				{
					Destroy(item);
				}
			}
		}

		foreach (GameObject item in items)
		{
			if (item.GetComponent<Collider2D>().IsTouching(coll2D))
			{
				if (Input.GetButton("Interact"))
				{
					Destroy(item);
				}
			}
		}
	}

	private void CheckIfTouchingEnemy()
	{
		foreach (GameObject enemy in enemies)
		{
			if (enemy.GetComponent<Collider2D>().IsTouching(coll2D))
			{
				Debug.Log("Colliding with enemy");
				FlashColor();
				//Code to add functionality when collision is detected, like attacking
			}
		}
	}

    private bool CheckIfGrounded()
    {
        isGrounded = Physics2D.Linecast(startOnPlayer.position, endOnGround.position, 1 << LayerMask.NameToLayer("Ground"));

        if(isGrounded)
        {
            anim.SetBool("OnGround", isGrounded);
            anim.SetFloat("vSpeed", 0);
        }
        else
        {
            anim.SetFloat("vSpeed", rb2D.velocity.y);
            anim.SetBool("OnGround", isGrounded);
            anim.Play("Jump/Fall");
        }

        return isGrounded;
    }
	#endregion

	#region Movement Functions

	private void FlipDirection()
	{
		facingRight = !facingRight;
		Vector2 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
	
	private void Jump()
	{
         rb2D.velocity = Vector2.up * jumpForce;
    }

	// <summary>
	// Causes the player to fall. The speed of the player's fall depends on how long they hold the Jump key. This allows
	// the user to either "short" jump or "long" jump. 
	// </summary>
	private void ApplyFallMultipliers()
	{
		if (rb2D.velocity.y < 0)
		{
			rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}
		else if (rb2D.velocity.y > 0 && !Input.GetButton("Jump"))
		{
			rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	/// <summary>
	/// Takes input from the user along the y axis and moves the player accordingly.
	/// </summary>
	private void MoveHorizontally()
	{
		float move = Input.GetAxis("Horizontal");

        if(move >= .2 || move <= -.2)
        {
            walking = true;
        }
        else
        {
            walking = false;
        }

		if (Input.GetButton("Sprint"))
		{
			rb2D.velocity = new Vector2(move * maxSpeed * 1.5f, rb2D.velocity.y);
		}
		else
		{
			rb2D.velocity = new Vector2(move * maxSpeed, rb2D.velocity.y);
            Walk();
        }

		// Flip the character if they're moving in the opposite direction
		if (move > 0 && !facingRight)
		{
			FlipDirection();
		}
		else if (move < 0 && facingRight)
		{
			FlipDirection();
		}
	}

	private void Stab()
	{
		anim.SetBool("stabbing", stabbing);
		anim.Play("Tory_Stabbing");
		stabbing = false;
		anim.SetBool("stabbing", stabbing);
	}

    private void Walk()
    {
        if(walking)
        {
            anim.SetBool("walking", walking);
        }
        else
        {
            anim.SetBool("walking", walking);
        }
    }

	private void FireWeapon()
	{
		Instantiate(bullet, bulletSpawn.position, Quaternion.identity);
	}


    IEnumerator FlashColor()
    {
        var normalColor = sr.material.color;

        sr.material.color = Color.red;
        yield return new WaitForSeconds(0.25F);

        sr.material.color = normalColor;
        yield return new WaitForSeconds(0.1F);
    }
    #endregion
}