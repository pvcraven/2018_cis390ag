using System.Collections;
using System.Collections.Generic;
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

	// If the character begins the level facing left, this needs to be changed to false.
	private bool facingRight = true;
	private bool isGrounded = true;
    private bool stabbing = false;
	#endregion

	#region Components
	Rigidbody2D rigidbody2D;
    Collider2D collider2D;
    GameObject[] food;
    GameObject[] weapons;
    GameObject[] items;
    GameObject[] enemies;
    Animator anim;
    #endregion

    void Start()
	{
        anim = GetComponent<Animator>();
		rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();

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
		CheckIfGrounded();
		ApplyFallMultipliers();
        CheckIfTouchingItems();
        CheckIfTouchingEnemy();
	}

    bool IsGrounded(){
       return Physics.Raycast(transform.position, -Vector3.down, groundDistance + 0.1f);
    }

// Use when applying physics-related functions. Runs in sync with the physics engine - may update 0, 1, or many times per frame depending on the physics FPS settings.
    void FixedUpdate()
	{
		CheckForInput();
        stabbing = Input.GetKeyDown("f");

        if(stabbing)
        {
            Stab();
            
        }
	}

	#region Logic Functions
	/// <summary>
	/// Checks for user input.
	/// </summary>
	private void CheckForInput()
	{
		if (Input.GetButton("Jump") && isGrounded)
		{
			Jump();
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

    /// <summary>
    /// Checks if the user is on the ground or not and modifies the isGrounded field accordingly.
    /// </summary>
    private void CheckIfGrounded()
	{
		if (rigidbody2D.velocity.y.Equals(0))
		{
			isGrounded = true;
		}
		else
		{
			isGrounded = false;
		}
	}
	#endregion
	
	#region Movement Functions
	/// <summary>
	/// Flips the player's sprite's direction.
	/// </summary>
	private void FlipDirection()
	{
		facingRight = !facingRight;
		Vector2 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	/// <summary>
	/// Causes the player to jump.
	/// </summary>
	private void Jump()
	{
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
        if(Input.GetButton("Sprint"))
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

    #endregion
}
