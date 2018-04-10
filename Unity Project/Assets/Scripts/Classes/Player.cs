using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ICharacterInterface {


	#region Properties
    public int Health{
		get {return health;}
		set {health = value;}}
	public bool Dead {
		get { return dead; }
		set { dead = value; }}
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

    public int WalkForce { get; private set; }

    private int SprintForce;

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
	private bool dead = false;
    private float stamina = 500;
	private int strength = 10;
	private int speed = 2;
	private bool isGrounded = false;
	private int jumpForce = 350;
    private int walkForce = 8;
    private int sprintForce = 16;
	private int fallMultiplier = 3;
	private int lowJumpMultiplier = 2;
	private bool facingRight = true;
	private bool walking = false;
	private string currentMeleeWeapon = null;
	private string currentRangedWeapon = null;
	private string currentAttackType = "melee";

    #endregion

    #region Components
    public GameObject player;
    private CapsuleCollider2D playerCC;

    public Transform startOnPlayer, endOnGround;

    public GameObject rangedAmmunition;
    public Transform rangedSpawner;

    private List<GameObject> food;
    private List<GameObject> weapons;
    private List<GameObject> items;
    private List<GameObject> water;
    private InventoryController invController;

    #endregion

    #region Contructor
    public Player(GameObject player){
        this.player = player;
		this.Health = 100;
        this.Stamina = 500;
		this.Strength = 0;
		this.Speed = 5;
		this.IsGrounded = true;
		this.FallMultiplier = 4;
		this.LowJumpMultiplier = 3;
		this.FacingRight = true;
		this.MeleeWeapon = null;
		this.RangedWeapon = null;
        water = new List<GameObject>(GameObject.FindGameObjectsWithTag("Water"));
        food = new List<GameObject>(GameObject.FindGameObjectsWithTag("Food"));
        weapons = new List<GameObject>(GameObject.FindGameObjectsWithTag("Weapon"));
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
            Vector2 jumpVelocity = new Vector2(0, jumpForce);
            rb.AddForce(jumpVelocity);
		}
    }

    public void Walk(float direction, float paceDistance = 0) {
        //Debug.Log("Walk2");

        // Check and see if we are paused
        if (Time.timeScale == 0)
            return;

        CheckDirection(direction);
		this.Walking = true;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        // Reduce the friction so we can move faster.
        rb.drag = 1f;
        Vector2 walkVector =new Vector2(direction * walkForce, 0);
        rb.AddForce(walkVector);

		player.GetComponent<Animator>().SetBool("walking", this.Walking);
    }
	
	public void StopMoving() {
		this.Walking = false;
        player.GetComponent<Rigidbody2D>().drag = 5;
		player.GetComponent<Animator>().SetBool("walking", this.Walking);}

    public void Sprint(float direction)
    {
        Debug.Log("Sprint");

        // Check and see if we are paused
        if (Time.timeScale == 0)
            return;

        CheckDirection(direction);
        this.Walking = true;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        // Reduce the friction so we can move faster.
        rb.drag = 1f;
        Vector2 walkVector = new Vector2(direction * sprintForce, 0);
        rb.AddForce(walkVector);

        player.GetComponent<Animator>().SetBool("walking", this.Walking);
    }
    public void Attack()
    {
        if (currentAttackType == "ranged")
        {
            RangedAttack();
            Debug.Log("Ranged attack");
        }
        else
        {
            MeleeAttack();
            Debug.Log("Melee attack");
        }
    }


    public void MeleeAttack(){
		Debug.Log("MeleeAttack with "+this.MeleeWeapon);

		switch(this.MeleeWeapon)
		{
			case "Knife" : 
				this.strength = 10;
				Stab(strength);
				break;
		}
    }

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
        Debug.Log("Interact");
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

        foreach (GameObject item in water)
        {
            var itemPickedUp = item.GetComponent<Collider2D>();
            var currentPlayer = player.GetComponent<Collider2D>();

            if (itemPickedUp.IsTouching(currentPlayer))
            {
                var addedItem = invController.AddItem(item);

                if (addedItem)
                {
                    water.Remove(item);
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
		}
    }

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
        }
    }
   
   public IEnumerator FlashColor()
    {
        var spriteRenderer = player.GetComponent<SpriteRenderer>();
        var normalColor = spriteRenderer.material.color;

        spriteRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.25F);

        spriteRenderer.material.color = normalColor;
        yield return new WaitForSeconds(0.1F);
    }

	private void Stab(int damage){
		player.GetComponent<Animator>().SetBool("stabbing", true);
		player.GetComponent<Animator>().Play("Tory_Stabbing");

        int position = 0;
        Collider2D collidingObject;
        playerCC = player.GetComponent<CapsuleCollider2D>();
        Collider2D[] overlappingObjects = Physics2D.OverlapCapsuleAll(new Vector2(playerCC.attachedRigidbody.position.x, playerCC.attachedRigidbody.position.y), new Vector2(playerCC.size.x + .05f, playerCC.size.y), playerCC.direction, 0);
        while (position < overlappingObjects.GetLength(0))
        {
            collidingObject = overlappingObjects[position];
            if (collidingObject.CompareTag("Enemy"))
            {
                ZombieController zombie = collidingObject.gameObject.GetComponent<ZombieController>();
                zombie.TakeDamage(damage);

            }
            position++;
        }

        player.GetComponent<Animator>().SetBool("stabbing", false);}

	public void Die() 
	{
		this.StopMoving ();
		player.GetComponent<Animator> ().SetBool ("dying", true);
		player.GetComponent<Animator> ().Play ("Tory_Dying");
	}

    public void ConsumeEdibleItem()
    {
        if (this.Stamina < 600)
            this.Stamina += 100;
    }

    public void UseHealthPack()
    {
        if (this.Health < 100)
            this.Health = 100;
    }
	#endregion
}
