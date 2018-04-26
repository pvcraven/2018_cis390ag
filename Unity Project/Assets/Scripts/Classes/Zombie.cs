using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : ICharacterInterface
{

    #region Properties
    public int Health
    {
        get
        {
            return this.health;
        }

        set
        {
            this.health = value;
        }
    }
    public int Speed
    {
        get
        {
            return this.speed;
        }

        set
        {
            this.speed = value;
        }
    }
    public int Strength
    {
        get
        {
            return this.strength;
        }

        set
        {
            this.strength = value;
        }
    }

    public bool IsGrounded
    {
        get { return isGrounded; }
        set { isGrounded = value; }
    }
    public bool FacingRight
    {
        get { return facingRight; }
        set { facingRight = value; }
    }
    public bool Walking
    {
        get { return walking; }
        set { walking = value; }
    }
    public bool CharacterFound
    {
        get { return this.characterFound; }
        set { this.characterFound = value; }
    }

    #endregion

    #region Variables

    private int health = 100;
    private int speed = 8;
    private int strength = 10;
    private bool facingRight = true;
    private bool isGrounded = false;
    private bool walking = false;
    public int jumpForce = 150;
    private Vector2 initialPosition;
    private bool characterFound = false;
    private System.Random rand = new System.Random();

    #endregion

    #region Components
    private GameObject zombie;

    public Transform sightStart, sightEnd;

    #endregion

    #region Constructor

    public Zombie(GameObject zombie) 
    {
        this.Health = 100;
        this.Speed = rand.Next(1, 3);
        this.Strength = 10;
        this.FacingRight = true;
        this.zombie = zombie;
        this.IsGrounded = true;
        this.initialPosition = zombie.GetComponent<Rigidbody2D>().position;
        this.characterFound = false;
        this.sightStart = zombie.GetComponentInChildren<Transform>();
        this.sightEnd = zombie.GetComponent<Transform>();
    }

    #endregion

    #region Methods
    public void CheckDirection(float direction)
    {
        // Flip the character if they're moving in the opposite direction
        if (direction > 0 && !facingRight)
        {
            FlipDirection();
        }
        else if (direction < 0 && facingRight)
        {
            FlipDirection();
        }
    }

    public void FlashColor()
    {

    }

    public void FlipDirection()
    {
        // Check and see if we are paused
        if (Time.timeScale == 0)
            return;

        this.FacingRight = !this.FacingRight;
        Vector2 scale = zombie.transform.localScale;
        scale.x *= -1;
        zombie.transform.localScale = scale;
        
    }

    public void TakeDamage(int damage)
    {
        this.health = this.health - damage;
        FlashColor();
    }

    public void Jump()
    {

        // Check and see if we are paused
        if (Time.timeScale == 0)
            return;

        // Check and see if we are on the ground

        Rigidbody2D rb = this.zombie.GetComponent<Rigidbody2D>();
        if (this.IsGrounded)
        {
            // Apply force to jump
            Vector2 jumpVelocity = new Vector2(0, jumpForce);
            rb.AddForce(jumpVelocity);
        }
    }

    public void GroundCheck()
    {

        this.IsGrounded = Physics2D.Linecast(this.zombie.GetComponent<ZombieController>().StartOnZombie.position, 
            this.zombie.GetComponent<ZombieController>().EndOnGround.position, 1 << LayerMask.NameToLayer("Ground"));
    }


    public void Walk(float direction = 1, float paceDistance = 0)
    {
        // Check and see if we are paused
        if (Time.timeScale == 0)
            return;

        //Debug.Log("C");

        if (!facingRight)
        {
            //Debug.Log("A");
            if(!CheckForPlayer())
            {
                zombie.transform.Translate(Vector2.right * this.Speed * Time.fixedDeltaTime);
            }
            else
            {
                zombie.transform.Translate(Vector2.right * this.Speed * 2 * Time.fixedDeltaTime);
            }
        }
        else
        {
            if(!CheckForPlayer())
            {
                zombie.transform.Translate(Vector2.left * this.Speed * Time.fixedDeltaTime);
            }
            else
            {
                zombie.transform.Translate(Vector2.left * this.Speed * 2 * Time.fixedDeltaTime);
            }
        }

        if(zombie.transform.position.x <= this.initialPosition.x - paceDistance)
        {
            //Debug.Log("stuff");

            if (facingRight)
            {
                FlipDirection();
            }
        }

        else if (zombie.transform.position.x >= initialPosition.x)
        {
            //Debug.Log("Morestuff");

            if (!facingRight)
            {
                FlipDirection();
            }
        }

        this.Walking = true;

        //zombie.GetComponent<Animator>().SetBool("walking", this.Walking);
    }
    bool CheckForPlayer()
    {
        return Physics2D.Linecast(sightStart.position, sightEnd.position, 1 << LayerMask.NameToLayer("Player"));
    }
    #endregion
}
