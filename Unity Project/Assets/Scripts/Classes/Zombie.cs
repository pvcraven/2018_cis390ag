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

    #endregion

    #region Variables

    private int health = 100;
    private int speed = 8;
    private int strength = 10;
    private bool facingRight = true;
    private bool walking = false;
    private float startingPosition = 0;

    #endregion

    #region Components
    public GameObject zombie;

    public Transform startOfLineOfSight, endOfLineOfSight;
    #endregion

    #region Constructor

    public Zombie(GameObject enemy) 
    {
        this.health = health;
        this.speed = speed;
        this.strength = strength;
        this.facingRight = true;
        this.zombie = zombie;
        this.startingPosition = zombie.GetComponent<Rigidbody2D>().position.x;
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
    }

    public void Walk(float direction = 1, float paceDistance = 0)
    {
        if (zombie.GetComponent<Rigidbody2D>().position.x >= this.startingPosition + paceDistance)
        {
            zombie.GetComponent<Rigidbody2D>().velocity = new Vector2(-direction * this.speed, zombie.GetComponent<Rigidbody2D>().velocity.y);
        }
        else if(zombie.GetComponent<Rigidbody2D>().position.x <= this.startingPosition - paceDistance)
        {
            zombie.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * this.speed, zombie.GetComponent<Rigidbody2D>().velocity.y);
        }

        CheckDirection(direction);

        this.Walking = true;

        zombie.GetComponent<Animator>().SetBool("walking", this.Walking);
    }

    #endregion
}
