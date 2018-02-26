using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
	#region Attributes
	public float maxSpeed = 5f;
	public float jumpForce = 300f;

	// If the character begins the level facing left, this needs to be modified
	private bool facingRight = true;
	// TODO: Add a more sophisticated "ground" checker
	private bool isGrounded = true;
	#endregion

	#region Components
	Rigidbody2D rigidbody2D;
	#endregion

	void Start()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	// Use when applying non-physics-related functions. Runs once per frame.
	void Update()
	{
		// Empty
	}

	// Use when applying physics-related functions. Runs in sync with the physics engine - may update 0, 1, or many times per frame depending on the physics FPS settings.
	void FixedUpdate()
	{
		CheckForInput();
	}

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
		rigidbody2D.AddForce(new Vector2(0, jumpForce));
		isGrounded = false;
	}

	private void MoveHorizontally()
	{
		// Takes input from the user along the y axis and moves the Player accordingly.
		float move = Input.GetAxis("Horizontal");
		rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

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
	#endregion

	#region Logic Functions
	private void CheckForInput()
	{
		if (Input.GetButton("Jump") && isGrounded) Jump();
		MoveHorizontally();
	}
	#endregion
}
