﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ICharacterInterface {


	#region Properties
    public int Health{
		get {return health;}
		set {health = value;}}
    public float Stamina{
        get { return stamina; }
        set { stamina = value; }}
    public int Strength {
		get {return strength;}
		set {strength = value;}}
	public int Speed {
		get{return speed;}
		set{speed = value;}}
	public bool IsGrounded{
		get{return isGrounded;}
		set{isGrounded = value;}}
	public int JumpForce{
		get{return jumpForce;}
		set{jumpForce = value;}}
	public int FallMultiplier{
		get{return fallMultiplier;}
		set{fallMultiplier = value;}}
	public int LowJumpMultiplier{
		get{return lowJumpMultiplier;}
		set{lowJumpMultiplier = value;}}
	public bool FacingRight{
		get{return facingRight;}
		set{facingRight = value;}}
	public bool Walking{
		get{return walking;}
		set{walking = value;}}
    public string MeleeWeapon{
		get{return currentMeleeWeapon;}
		set{currentMeleeWeapon = value;}}
	public string RangedWeapon{
		get{return currentRangedWeapon;}
		set{currentRangedWeapon = value;}}
    public string CurrentAttackType
    {
        get { return currentAttackType; }
        set { currentAttackType = value; }
    }

    #endregion

    #region Variables
    private int health = 100;
    private float stamina = 500;
	private int strength = 10;
	private int speed = 2;
	private bool isGrounded = false;
	private int jumpForce = 7;
	private int fallMultiplier = 3;
	private int lowJumpMultiplier = 2;
	private bool facingRight = true;
	private bool walking = false;
	private string currentMeleeWeapon = "";
	private string currentRangedWeapon = "";
	private string currentAttackType = "melee";

    #endregion

    #region Components
    public GameObject player;

    public Transform startOnPlayer, endOnGround;

    public GameObject rangedAmmunition;
    public Transform rangedSpawner;

    //private GameObject[] food;
    private List<GameObject> food;
    //private GameObject[] weapons;
    private List<GameObject> weapons;
    //private GameObject[] items;
    private List<GameObject> items;
    private InventoryController invController;

    #endregion

    #region Contrustor
    public Player(GameObject player){

		this.Health = 100;
        this.Stamina = 500;
		this.Strength = 0;
		this.Speed = 5;
		this.IsGrounded = true;
        this.JumpForce = 12;
		this.FallMultiplier = 4;
		this.LowJumpMultiplier = 3;
		this.FacingRight = true;
        this.player = player;
		this.MeleeWeapon = "Knife";
		this.RangedWeapon = "Gun";

        food = new List<GameObject>(GameObject.FindGameObjectsWithTag("Food"));
        //weapons = GameObject.FindGameObjectsWithTag("Weapon");
        weapons = new List<GameObject>(GameObject.FindGameObjectsWithTag("Weapon"));
        //items = GameObject.FindGameObjectsWithTag("Item");
        items = new List<GameObject>(GameObject.FindGameObjectsWithTag("Item"));
        invController = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryController>();
    }

	#endregion
	
	#region Methods 
	public void Jump(){

        // Check and see if we are paused
        if (Time.timeScale == 0)
            return;

        // Check and see if we are on the ground
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (this.IsGrounded)
		{
            // Apply force to jump
            Vector2 jumpVelocity = new Vector2(0, 350);
            rb.AddForce(jumpVelocity);
		}
    }

    public void Walk(float direction, float paceDistance = 0) {
        // Check and see if we are paused
        if (Time.timeScale == 0)
            return;

        CheckDirection(direction);
		this.Walking = true;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        // Reduce the friction so we can move faster.
        rb.drag = 1f;
        Vector2 walkVector =new Vector2(direction * 8, 0);
        rb.AddForce(walkVector);

		player.GetComponent<Animator>().SetBool("walking", this.Walking);}
	
	public void StopMoving() {
		this.Walking = false;
        player.GetComponent<Rigidbody2D>().drag = 5;
		player.GetComponent<Animator>().SetBool("walking", this.Walking);}

	public void Sprint(float direction){
		CheckDirection(direction);
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * speed * 1.5f, player.GetComponent<Rigidbody2D>().velocity.y);}

    public void Attack()
    {
        if (currentAttackType == "ranged")
            RangedAttack();
        else
            MeleeAttack();
    }

	public void MeleeAttack(){
		//Debug.Log("MeleeAttack");

		switch(this.MeleeWeapon)
		{
			case "Knife" : 
				this.strength = 10;
				Stab();
				break;
		}}

    public void switchWeapon()
    {
        //Debug.Log("SwitchWeapon");
        if(currentAttackType == "melee")
        {
            currentAttackType = "ranged";
        }
        else
        {
            currentAttackType = "melee";
        }
        //Debug.Log("New Weapon: " + currentAttackType);
    }

	public void RangedAttack(){
		//Debug.Log("RangedAttack");

		this.rangedAmmunition = player.GetComponent<PlayerController>().rangedAmmunition;
		this.rangedSpawner =  player.GetComponent<PlayerController>().rangedSpawner;
		
		switch(this.RangedWeapon)
		{
			case "Gun" : 
				this.strength = 50;

				//This needs to work with a Bullet Class rather than the previously existing way.
				//Reason: It will only work with a gun, the previously existing way prohibits any future ranged weapons.
				//Also: the current bullets never despawn, are pretty slow, 
				//		and can only be fired in a straight line toward the positve X axis

				//rangedAmmunition = new Gun();
				break;
		}

        if (this.RangedWeapon != null)
        {
            GameObject shot = UnityEngine.Object.Instantiate(rangedAmmunition, rangedSpawner.position, Quaternion.identity);
            Rigidbody2D shotRB = shot.GetComponent<Rigidbody2D>();

            //Potentially move to class
            if (this.FacingRight)
            {
                Vector2 scale = shot.transform.localScale;
                scale.x *= -1;
                shot.transform.localScale = scale;
                shotRB = shot.GetComponent<Rigidbody2D>();
                shotRB.AddForce(new Vector2(500, 0));
            }
            else
            {
                shotRB = shot.GetComponent<Rigidbody2D>();
                shotRB.AddForce(new Vector2(-500, 0));
            }

            UnityEngine.Object.Destroy(shot, 3.0f);
        }
    }
		
	public GameObject Interact()
    {
        foreach (GameObject item in food)
        {
            var itemPickedUp = item.GetComponent<Collider2D>();
            var currentPlayer = player.GetComponent<Collider2D>();

            if (itemPickedUp.IsTouching(currentPlayer))
            {
                var addedItem = invController.AddItem(item);

                if (addedItem)
                {
                    food.Remove(item);
                    return item;
                }
                else if (!addedItem)
                {
                    return null;
                }
            }
        }

        foreach (GameObject item in weapons)
        {
            var itemPickedUp = item.GetComponent<Collider2D>();
            var currentPlayer = player.GetComponent<Collider2D>();

            if (itemPickedUp.IsTouching(currentPlayer))
            {
                var addedItem = invController.AddItem(item);

                if (addedItem)
                {
                    weapons.Remove(item);
                    return item;
                }
                else if (!addedItem)
                {
                    return null;
                }
            }
        }

        foreach (GameObject item in items)
        {
            var itemPickedUp = item.GetComponent<Collider2D>();
            var currentPlayer = player.GetComponent<Collider2D>();

            if (itemPickedUp.IsTouching(currentPlayer))
            {
                var addedItem = invController.AddItem(item);

                if (addedItem)
                {
                    items.Remove(item);
                    return item;
                }
                else if (!addedItem)
                {
                    return null;
                }
            }
        }

        return null;
    }

	public void CheckDirection(float direction) {
		// Flip the character if they're moving in the opposite direction
		if (direction > 0 && !facingRight)
		{
			FlipDirection();
		}
		else if (direction < 0 && facingRight)
		{
			FlipDirection();
		}}

	public void FlipDirection() {
		FacingRight = !FacingRight;
		Vector2 scale = player.transform.localScale;
		scale.x *= -1;
		player.transform.localScale = scale;}

	public void TakeDamage(int damage) {
		this.health = this.health - damage;}

    public void GroundCheck(){

		this.IsGrounded = Physics2D.Linecast(this.player.GetComponent<PlayerController>().startOnPlayer.position, 
											 this.player.GetComponent<PlayerController>().endOnGround.position, 
											 1 << LayerMask.NameToLayer("Ground"));
        
        if (this.IsGrounded)
        {
            player.GetComponent<Animator>().SetBool("OnGround", this.IsGrounded);
            player.GetComponent<Animator>().SetFloat("vSpeed", 0);
        }
        else
        {
            player.GetComponent<Animator>().SetFloat("vSpeed", player.GetComponent<Rigidbody2D>().velocity.y);
            player.GetComponent<Animator>().SetBool("OnGround", this.IsGrounded);
            player.GetComponent<Animator>().Play("Jump/Fall");
        }}
   
   public IEnumerator FlashColor()
    {
        var spriteRenderer = player.GetComponent<SpriteRenderer>();
        var normalColor = spriteRenderer.material.color;

        spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.25F);

        spriteRenderer.material.color = normalColor;
        yield return new WaitForSeconds(0.1F);
    }

	private void Stab(){
		player.GetComponent<Animator>().SetBool("stabbing", true);
		player.GetComponent<Animator>().Play("Tory_Stabbing");
		player.GetComponent<Animator>().SetBool("stabbing", false);}

	#endregion
}
