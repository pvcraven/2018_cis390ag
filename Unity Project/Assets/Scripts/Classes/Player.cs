using System;
using System.Collections;
using System.Collections.Generic;
using Classes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : ICharacterInterface
{
	#region Properties

	public Weapon Weapon
	{
		get { return weapon; }
		set { weapon = value; }
	}
	
	public int Health
	{
		get { return this.health; }
		set { this.health = value; }
	}

	public bool Dead
	{
		get { return dead; }
		set { dead = value; }
	}

	public float Stamina
	{
		get { return stamina; }
		set { stamina = value; }
	}

	public int Strength
	{
		get { return strength; }
		set { strength = value; }
	}

	public int Speed
	{
		get { return speed; }
		set { speed = value; }
	}

	public bool IsGrounded
	{
		get { return isGrounded; }
		set { isGrounded = value; }
	}

	public int JumpForce
	{
		get { return jumpForce; }
		set { jumpForce = value; }
	}

	public int WalkForce { get; private set; }
	private int SprintForce;

	public int FallMultiplier
	{
		get { return fallMultiplier; }
		set { fallMultiplier = value; }
	}

	public int LowJumpMultiplier
	{
		get { return lowJumpMultiplier; }
		set { lowJumpMultiplier = value; }
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

	public string MeleeWeapon
	{
		get { return currentMeleeWeapon; }
		set { currentMeleeWeapon = value; }
	}

	public string RangedWeapon
	{
		get { return currentRangedWeapon; }
		set { currentRangedWeapon = value; }
	}

	public string CurrentAttackType
	{
		get { return currentAttackType; }
		set { currentAttackType = value; }
	}

    public bool Stabbing
    {
        get { return stabbing; }
        set { stabbing = value; }
    }

    public IDictionary<string, string> GetStatusBarInformation
	{
		get
		{
			statusBarInformation.Clear();
			statusBarInformation.Add("Health", Health.ToString());
			statusBarInformation.Add("Stamina", Stamina.ToString());
			statusBarInformation.Add("AttackType", CurrentAttackType);
			statusBarInformation.Add("Strength", Strength.ToString());
			return statusBarInformation;
		}
	}



	#endregion

	#region VariablesstartOnPlayer

	private int health = 100;
	private bool dead = false;
	private float stamina = 500;
	private int strength = 10;
	private int speed = 2;
	private bool isGrounded = false;
	private int jumpForce = 500;
	private int walkForce = 15;
	private int sprintForce = 20;
    private bool stabbing = false;
	private int fallMultiplier = 3;
	private int lowJumpMultiplier = 2;
	private bool facingRight = true;
	private bool walking = false;
	private string currentMeleeWeapon = null;
	private string currentRangedWeapon = null;
	private string currentAttackType = "melee";
	private IDictionary<string, string> statusBarInformation = new Dictionary<string, string>();
	private Weapon weapon = new Weapon();

	#endregion

	#region Components

	public GameObject player;
	private CapsuleCollider2D playerCC;

	public Transform StartOnPlayer, EndOnGround;

	public GameObject rangedAmmunition;
	public Transform rangedSpawner;

	private List<GameObject> food;
	private List<GameObject> weapons;
	private List<GameObject> items;
	private List<GameObject> water;
	private InventoryController invController;

	#endregion

	#region Contructor

	public Player(GameObject player)
	{
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

		items.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("HealthPack")));
	}

	#endregion

	#region Methods 

	public void Jump()
	{

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

	public void Walk(float direction, float paceDistance = 0)
	{

		// Check and see if we are paused
		if (Time.timeScale == 0)
			return;

		CheckDirection(direction);
		this.Walking = true;
		Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

		// Reduce the friction so we can move faster.
		Vector2 walkVector = new Vector2(direction * walkForce, rb.velocity.y);

		// If the player is moving faster than walkforce, their velocity gets reset to walkforce.
        // walkForce also functions as the player's maximum possible walk speed.
		if (rb.velocity.x < -walkForce)
            walkVector.x = -walkForce;
		else if (rb.velocity.x > walkForce)
            walkVector.x = walkForce;

		// The ground slows Tory due to friction. This makes them slightly faster. Same for ramps.
		if (this.isGrounded)
		{
			if (rb.velocity.y == 0)
                walkVector.x *= 1.2f;
			else if (rb.velocity.y > 0.01f)
                walkVector.x *= 1.5f;
		}
        
		rb.AddForce(walkVector);

		player.GetComponent<Animator>().SetBool("walking", this.Walking);
	}

	public void StopMoving()
	{
		this.Walking = false;
		player.GetComponent<Rigidbody2D>().drag = 5;
		player.GetComponent<Animator>().SetBool("walking", this.Walking);
	}

	public void Sprint(float direction)
	{
		// Check and see if we are paused
		if (Time.timeScale == 0)
			return;

		CheckDirection(direction);
		this.Walking = true;
		Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
		Vector2 walkVector = new Vector2(direction * sprintForce, rb.velocity.y);

		// If the player moves faster than sprintforce, their velocity gets reset to sprintforce.
        // sprintForce also functions as the player's maxmimum possible sprint speed.
		if (rb.velocity.x < -sprintForce)
            walkVector.x = -sprintForce;
		else if (rb.velocity.x > sprintForce)
            walkVector.x = sprintForce;

		// The ground slows Tory due to friction. This makes them slightly faster. Same for ramps.
		if (this.isGrounded)
		{
			if (rb.velocity.y == 0)
                walkVector.x *= 1.2f;
			else if (rb.velocity.y > 0.01f)
                walkVector.x *= 1.5f;
		}

		rb.AddForce(walkVector);

		player.GetComponent<Animator>().SetBool("walking", this.Walking);
	}

	public void Attack()
	{
		if (invController.WeaponIsInInventory())
		{
			if (currentAttackType == "ranged")
			{
                this.RangedWeapon = "Gun"; //Ideally, if we had more than one type of ranged or melee weapon we would change these in the inventory.
                RangedAttack();
				//Debug.Log("Ranged attack"); 
			}
			else
			{
                this.MeleeWeapon = "Knife";
                MeleeAttack();
				//Debug.Log("Melee attack");
            }
		}
	}


	public void MeleeAttack()
	{
		//Debug.Log("MeleeAttack with "+ this.MeleeWeapon);

		switch (this.MeleeWeapon)
		{
			case "Knife":
				this.strength = 10;
				Stab(strength);
				break;
		}
	}

	public void SwitchWeapon()
	{
		if (invController.AbleToSwitchWeapons())
		{
			//Debug.Log("SwitchWeapon");
			if (currentAttackType == "melee")
			{
				currentAttackType = "ranged";
			}
			else
			{
				currentAttackType = "melee";
			}

			this.player.GetComponent<StatusBarLogic>().SetWeapon();
		}
	}

	public void RangedAttack()
	{
		//Debug.Log("RangedAttack");

		this.rangedAmmunition = player.GetComponent<PlayerController>().rangedAmmunition;
		this.rangedSpawner = player.GetComponent<PlayerController>().rangedSpawner;

		switch (this.RangedWeapon)
		{
			case "Gun":
				this.strength = 50;

				//This needs to work with a Bullet or Gun Class rather than the previously existing way.
				//Reason: It will only work with a gun, the previously existing way prohibits any future ranged weapons.

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

	public GameObject Interact(AudioClip clip)
	{
		foreach (GameObject item in food)
		{
			var touching = PlayerIsTouchingItem(item);
			if (touching)
				return InteractWithObject(item, food, clip);
		}

		foreach (GameObject item in water)
		{
			var touching = PlayerIsTouchingItem(item);
			if (touching)
				return InteractWithObject(item, water, clip);
		}

		foreach (GameObject item in weapons)
		{
			var touching = PlayerIsTouchingItem(item);
			if (touching)
			{
				this.player.GetComponent<StatusBarLogic>().SetWeapon();
				//Debug.Log("item " + item);
				//Debug.Log("Weapons " + weapons.ToArray().ToString());
				return InteractWithObject(item, weapons, clip);
			}
		}

		foreach (GameObject item in items)
		{
			var touching = PlayerIsTouchingItem(item);
			if (touching)
				return InteractWithObject(item, items, clip);
		}

		return null;
	}

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

	public void FlipDirection()
	{
		FacingRight = !FacingRight;
		Vector2 scale = player.transform.localScale;
		scale.x *= -1;
		player.transform.localScale = scale;
	}

	public void TakeDamage(int damage)
	{
		this.health -= damage;
		this.player.GetComponent<StatusBarLogic>().SetHealth();
        if(this.Health <= 0)
        {
            //Debug.Log("Die");
            this.Die();
        }
	}

    public void AdjustStamina(float stamina)
    {
        this.stamina += stamina;
        this.player.GetComponent<StatusBarLogic>().SetStamina();
    }

	public void GroundCheck()
	{

		this.IsGrounded = Physics2D.Linecast(this.player.GetComponent<PlayerController>().StartOnPlayer.position,
			this.player.GetComponent<PlayerController>().EndOnGround.position,
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

    
    //WAAAAAAY to complicated for what we are doing... I can't read this to I can't fix it.

	//private void Stab(int damage)
	//{
	//	float MeleeAttackHitBox = 0;

 //       this.Stabbing = true;

	//	player.GetComponent<Animator>().SetBool("stabbing", true);

	//	int position = 0;
	//	Collider2D collidingObject;
	//	playerCC = player.GetComponent<CapsuleCollider2D>();
	//	if (facingRight)
	//	{
	//		MeleeAttackHitBox = playerCC.attachedRigidbody.position.x + 1;
	//	}
	//	else
	//	{
	//		MeleeAttackHitBox = playerCC.attachedRigidbody.position.x - 1;
	//	}

	//	Collider2D[] overlappingObjects = Physics2D.OverlapCapsuleAll(
	//		new Vector2(MeleeAttackHitBox, playerCC.attachedRigidbody.position.y),
	//		new Vector2(playerCC.size.x + .05f, playerCC.size.y), playerCC.direction, 0);
	//	while (position < overlappingObjects.GetLength(0))
	//	{
	//		collidingObject = overlappingObjects[position];
	//		if (collidingObject.CompareTag("Zombie"))
	//		{
	//			Zombie zombie = collidingObject.gameObject.GetComponent<Zombie>();
	//			zombie.TakeDamage(damage);

	//		}

	//		position++;
	//	}

 //       PlayerController script = player.GetComponent<PlayerController>();
 //       script.MeleeAnimationDelay();

 //       this.Stabbing = false;

 //   }
   public void SetAnimationFalse()
    {
        player.GetComponent<Animator>().SetBool("stabbing", false);
    }	
 

    public void Die() 
	{
        this.StopMoving ();
        this.sprintForce = 0;
        this.walkForce = 0;
		player.GetComponent<Animator>().SetBool ("dying", true);
        SceneManager.LoadScene("Dead");
	}

    public void ConsumeEdibleItem()
    {
        if (this.Stamina < 400)
        {
            this.Stamina += 100;
        }
        else if(this.Stamina < 500)
        {
            this.Stamina = 500;
        }
    }

    public void UseHealthPack()
    {
        if (this.Health < 100)
        {
            this.Health = 100;
        }
    }

    private GameObject InteractWithObject(GameObject item, List<GameObject> inArray, AudioClip clip)
    {
        var addedItem = invController.AddItem(item);

        if(addedItem)
        {
            AudioSource.PlayClipAtPoint(clip, player.transform.position);
            inArray.Remove(item);

            if(currentMeleeWeapon == null)
            {
                if(item.name.Contains("Knife"))
                {
                    MeleeWeapon = "Knife";
                }
            }

            return item;
        }

        return null;
    }

    private bool PlayerIsTouchingItem(GameObject item)
    {
        var itemPickedUp = item.GetComponent<Collider2D>();
        var currentPlayer = player.GetComponent<Collider2D>();

        var touching = itemPickedUp.IsTouching(currentPlayer);
        return touching;
    }
    #endregion
}
