using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
	#region Attributes

	public float maxSpeed = 5f;
	public float sprintMultiplier = 1.5f;
	public float jumpForce = 5f;
	public float fallMultiplier = 2f;
	public float lowJumpMultiplier = 2f;
	public float fireRate = 1f;

    // If the character begins the level facing left, this needs to be changed to false.
    private bool isFacingRight = true;
	private bool isGrounded = false;

    //Anim Variables
    private bool isStabbing = false;
    private bool isWalking = false;
    private float vSpeed = 0f;
    private float health;
    private float stamina;

	private bool hasWeapon = true;
	private float nextFire;

	#endregion

	#region Components

	private GameObject[] food;
	private GameObject[] weapons;
	private GameObject[] items;
	private GameObject[] enemies;
	Animator anim;
	private Rigidbody2D rb2D;
	private Collider2D coll2D;
    private AudioSource audioSource;
	private SpriteRenderer sr;
	public GameObject bullet;
	public Transform bulletSpawn;
	public Transform startOnPlayer, endOnGround;

	#endregion

	void Start()
	{
		// Initialize components
		anim = GetComponent<Animator>();
		rb2D = GetComponent<Rigidbody2D>();
		coll2D = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();

		food = GameObject.FindGameObjectsWithTag("Food");
		weapons = GameObject.FindGameObjectsWithTag("Weapon");
		items = GameObject.FindGameObjectsWithTag("Item");
		enemies = GameObject.FindGameObjectsWithTag("Enemy");

		//TODO: Check if this is needed since it's in the player class
        this.stamina = 100f;
        this.health = 100f;
        Debug.Log(this.stamina);
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
            audioSource.Play();
		}

        isStabbing = Input.GetKeyDown("f");

        if (isStabbing)
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
		isFacingRight = !isFacingRight;
		Vector2 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
	
	private void Jump()
	{
         rb2D.velocity = Vector2.up * jumpForce;
    }

	// Adjusts the player's jump depending on how long they hold the jump key
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

	private void MoveHorizontally()
	{
		float move = Input.GetAxis("Horizontal");

        if(move >= .2 || move <= -.2)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

		if (Input.GetButton("Sprint"))
		{
			rb2D.velocity = new Vector2(move * maxSpeed * sprintMultiplier, rb2D.velocity.y);
            stamina -= 1f;
            Debug.Log(this.stamina);
        }
		else
		{
			rb2D.velocity = new Vector2(move * maxSpeed, rb2D.velocity.y);
            stamina += .5f;
            Debug.Log(this.stamina);
            Walk();
        }

		// Flip the character if they're moving in the opposite direction
		if (move > 0 && !isFacingRight)
		{
			FlipDirection();
		}
		else if (move < 0 && isFacingRight)
		{
			FlipDirection();
		}
	}

	private void Stab()
	{
		anim.SetBool("stabbing", isStabbing);
		anim.Play("Tory_Stabbing");
		isStabbing = false;
		anim.SetBool("stabbing", isStabbing);
	}

    private void Walk()
    {
        if(isWalking)
        {
            anim.SetBool("walking", isWalking);
        }
        else
        {
            anim.SetBool("walking", isWalking);
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