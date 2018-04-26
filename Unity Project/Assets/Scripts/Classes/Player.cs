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
	private int jumpForce = 450;
	private int walkForce = 15;
	private int sprintForce = 20;
    private Animator anim;
    private int fallMultiplier = 3;
	private int lowJumpMultiplier = 2;
	private bool facingRight = true;
	private bool walking = false;
	private string currentMeleeWeapon = null;
	private string currentRangedWeapon = null;
	private string currentAttackType = "melee";
	private IDictionary<string, string> statusBarInformation = new Dictionary<string, string>();
	private Weapon weapon = new Weapon();
    public float color_flash_timer = 0;

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
            rb.drag = 1;
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
		Vector2 walkVector = new Vector2(direction * walkForce, rb.velocity.y);

        if (this.IsGrounded && rb.velocity.y > 0.01f)
            walkVector.x *= 1.2f;

        if (!walkingTooFast())
            rb.AddForce(walkVector);

		player.GetComponent<Animator>().SetBool("walking", this.Walking);
	}

    private bool walkingTooFast()
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        return Mathf.Abs(rb.velocity.x) > 3;
    }
     
    private bool runningTooFast()
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        return Mathf.Abs(rb.velocity.x) > 5;
    }

    public void StopMoving()
	{
		this.Walking = false;
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
		Vector2 sprintVector = new Vector2(direction * sprintForce, rb.velocity.y);

        if (this.IsGrounded && rb.velocity.y > 0.01f)
            sprintVector.x *= 1.2f;

        if (!runningTooFast())
            rb.AddForce(sprintVector);

		player.GetComponent<Animator>().SetBool("walking", this.Walking);
	}

	public void Attack(AudioClip clip)
	{
		if (invController.WeaponIsInInventory())
		{
			if (currentAttackType == "ranged")
			{
                this.RangedWeapon = "Gun"; //Ideally, if we had more than one type of ranged or melee weapon we would change these in the inventory.
                RangedAttack();
				AudioSource.PlayClipAtPoint(clip, player.transform.position, 0.25f);
				//Debug.Log("Ranged attack"); 

			}
			else
			{
                this.MeleeWeapon = "Knife";
                MeleeAttack();
				AudioSource.PlayClipAtPoint(clip, player.transform.position, 0.25f);
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
                Stab(this.strength);
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
                Shoot();

				//This needs to work with a Bullet or Gun Class rather than the previously existing way.
				//Reason: It will only work with a gun, the previously existing way prohibits any future ranged weapons.

				//rangedAmmunition = new Gun();
				break;
		}

		if (this.RangedWeapon != null)
		{
            if (Time.timeScale == 0)
                return;
            else
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

    public void FlashColor()
    {
        var spriteRenderer = player.GetComponent<SpriteRenderer>();
        if (color_flash_timer > 0)
        {
            spriteRenderer.material.color = Color.red;
            color_flash_timer -= Time.deltaTime;
        }
        else
        {
            spriteRenderer.material.color = Color.white;
        }
	}

    
	private void Stab(int damage)
	{
        this.strength = 10;
        anim = player.GetComponent<Animator>();
        anim.SetBool("stabbing", true);
        anim.Play("Tory_Stabbing");
        anim.SetBool("stabbing", false);

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        // Loop through each bandit. 
        // This needs to be refactored so it works for ANY enemy and
        // not duplicate this code for each type of enemy.
        foreach (BanditEnemyController e in UnityEngine.Object.FindObjectsOfType<BanditEnemyController>())
        {
            // This isn't great, because you should only stab the direction
            // you are facing. And this code doesn't care about that. But
            // we just need to get something down.
            if (Vector2.Distance(e.transform.position, rb.position) < 1)
            {
                // An enemy is in your radius
                e.TakeDamage(10);
            }
        }

        // Loop through each zombie. 
        // This needs to be refactored so it works for ANY enemy and
        // not duplicate this code for each type of enemy.
        foreach (ZombieController e in UnityEngine.Object.FindObjectsOfType<ZombieController>())
        {
            // This isn't great, because you should only stab the direction
            // you are facing. And this code doesn't care about that. But
            // we just need to get something down.
            if (Vector2.Distance(e.transform.position, rb.position) < 1.25f)
            {
                // Debug.Log("Zombie is close");
                // An enemy is in your radius
                e.TakeDamage(10);
            }
            else
            {
                // Debug.Log("Zombie is not close: " + Vector2.Distance(e.transform.position, rb.position));
            }
        }
        // Debug.Log("Done processing hits");
    }

    public void Shoot()
    {
        anim = player.GetComponent<Animator>();
        anim.SetBool("shooting", true);
        anim.Play("Tory_Shooting");
        anim.SetBool("shooting", false);
    }

    public void SetAnimationFalse()
    {
        //player.GetComponent<Animator>().SetBool("stabbing", false);
        //player.GetComponent<Animator>().SetBool("shooting", false);
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
        this.player.GetComponent<StatusBarLogic>().SetHealth();
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
