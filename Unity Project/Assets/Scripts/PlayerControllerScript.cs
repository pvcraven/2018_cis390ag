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

	// If the character begins the level facing left, this needs to be changed to false.
	private bool facingRight = true;
	private bool isGrounded = false;
	private bool stabbing = false;
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
	private Rigidbody2D rigidbody2D;
	private Collider2D collider2D;

	#endregion

	void Start()
	{
		anim = GetComponent<Animator>();
		rigidbody2D = GetComponent<Rigidbody2D>();
		collider2D = GetComponent<Collider2D>();
		contactFilter.useTriggers = false;
		contactFilter.SetLayerMask (Physics2D.GetLayerCollisionMask (gameObject.layer));
		contactFilter.useLayerMask = true;

		food = GameObject.FindGameObjectsWithTag("Food");
		weapons = GameObject.FindGameObjectsWithTag("Weapon");
		items = GameObject.FindGameObjectsWithTag("Item");
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
	}

	// Use when applying non-physics-related functions. Runs once per frame.
	void Update()
	{
		CheckForInput();

		// Falling
		//CheckIfGrounded();
		ApplyFallMultipliers();
		CheckIfTouchingItems();
		CheckIfTouchingEnemy();
		CheckIfGrounded();
	}

// Use when applying physics-related functions. Runs in sync with the physics engine - may update 0, 1, or many times per frame depending on the physics FPS settings.
	void FixedUpdate()
	{
		CheckForInput();
		stabbing = Input.GetKeyDown("f");

		if (stabbing)
		{
			Stab();
		}
	}

	#region Logic Functions

	private void CheckForInput()
	{
		if (Input.GetButtonDown("Jump") && this.isGrounded)
		{
			Jump();
		}

		if (Input.GetButton("Shoot") && hasWeapon && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			FireWeapon();
		}

		MoveHorizontally();
	}

	private void CheckIfTouchingItems()
	{
		foreach (GameObject item in food)
		{
			if (item.GetComponent<Collider2D>().IsTouching(collider2D))
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
			if (item.GetComponent<Collider2D>().IsTouching(collider2D))
			{
				if (Input.GetButton("Interact"))
				{
					Destroy(item);
				}
			}
		}

		foreach (GameObject item in items)
		{
			if (item.GetComponent<Collider2D>().IsTouching(collider2D))
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
			if (enemy.GetComponent<Collider2D>().IsTouching(collider2D))
			{
				Debug.Log("Colliding with enemy");
				//Code to add functionality when collision is detected, like attacking
			}
		}
	}

	private void CheckIfGrounded()
	{
		this.isGrounded = false;
		int count = rigidbody2D.Cast (new Vector2(0, -1), contactFilter, hitBuffer, .2f);
		hitBufferList.Clear();

		foreach (RaycastHit2D element in hitBuffer)
		{
			hitBufferList.Add(element);
		}

		for (int i = 0; i < hitBufferList.Count; i++) 
		{
			Vector2 currentNormal = hitBufferList[i].normal;
			if (currentNormal.y > .01f) 
			{
				this.isGrounded = true;
			}
		}
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
		this.isGrounded = false;
		rigidbody2D.velocity = Vector2.up * jumpForce;
	}

	/// <summary>
	/// Causes the player to fall. The speed of the player's fall depends on how long they hold the Jump key. This allows
	/// the user to either "short" jump or "long" jump. 
	/// </summary>
	private void ApplyFallMultipliers()
	{
		if (rigidbody2D.velocity.y < 0)
		{
			rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}
		else if (rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
		{
			rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	/// <summary>
	/// Takes input from the user along the y axis and moves the player accordingly.
	/// </summary>
	private void MoveHorizontally()
	{
		float move = Input.GetAxis("Horizontal");
		if (Input.GetButton("Sprint"))
		{
			rigidbody2D.velocity = new Vector2(move * maxSpeed * 1.5f, rigidbody2D.velocity.y);
		}
		else
		{
			rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
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

	private void FireWeapon()
	{
		Instantiate(bullet, bulletSpawn.position, Quaternion.identity);
	}

	#endregion
}