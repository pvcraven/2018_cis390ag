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
    private bool walking = false;
    private float startingPosition = 0;
    private bool characterFound = false;

    #endregion

    #region Components
    private GameObject zombie;

    public Transform sightStart, sightEnd;
    #endregion

    #region Constructor

    public Zombie(GameObject zombie) 
    {
        this.Health = 100;
        this.Speed = 8;
        this.Strength = 10;
        this.FacingRight = true;
        this.zombie = zombie;
        this.startingPosition = zombie.GetComponent<Rigidbody2D>().position.x;
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

    public IEnumerator FlashColor()
    {
        var spriteRenderer = zombie.GetComponent<SpriteRenderer>();
        var normalColor = spriteRenderer.material.color;

        spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.25F);

        spriteRenderer.material.color = normalColor;
        yield return new WaitForSeconds(0.1F);
    }

    public void FlipDirection()
    {
        FacingRight = !FacingRight;
        Vector2 scale = zombie.transform.localScale;
        scale.x *= -1;
        zombie.transform.localScale = scale;
    }

    public void TakeDamage(int damage)
    {
        this.health = this.health - damage;
        FlashColor();
    }

    public void Walk(float direction = 1, float paceDistance = 0)
    {
        this.characterFound = CheckForPlayer();

        if (!characterFound)
        {
            if (zombie.GetComponent<Rigidbody2D>().position.x >= this.startingPosition + paceDistance)
            {
                zombie.GetComponent<Rigidbody2D>().velocity = new Vector2(-direction * this.speed, zombie.GetComponent<Rigidbody2D>().velocity.y);
            }
            else if (zombie.GetComponent<Rigidbody2D>().position.x <= this.startingPosition - paceDistance)
            {
                zombie.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * this.speed, zombie.GetComponent<Rigidbody2D>().velocity.y);
            }

            CheckDirection(direction);
        }
        else
        {
            Debug.Log("Player found!");
        }

        this.Walking = true;

        zombie.GetComponent<Animator>().SetBool("walking", this.Walking);
    }
    bool CheckForPlayer()
    {
        return Physics2D.Linecast(sightStart.position, sightEnd.position, 1 << LayerMask.NameToLayer("Player"));
    }
    #endregion
}
