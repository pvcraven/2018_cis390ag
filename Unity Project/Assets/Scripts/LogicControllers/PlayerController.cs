using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public KeyCode jumpKey = KeyCode.Space;
	public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode attack = KeyCode.F;
	public KeyCode interactKey = KeyCode.R;
	public KeyCode pauseKey = KeyCode.Escape;
    public KeyCode switchWeapon = KeyCode.LeftControl;

	public GameObject bullet;
	public Transform bulletSpawn;
    private AudioSource gunShot;

	private bool walk;

    public GameObject player;
	public GameObject rangedAmmunition;
	public Transform rangedSpawner;
    public Transform startOnPlayer, endOnGround;
    public Player tory;

    private float direction = 0;
    private SpriteRenderer spriteRend;

	void Start(){
        tory = new Player(player);
        gunShot = GetComponent<AudioSource>();
	}

	void Update(){
		CheckforInput();
        if (Input.GetKey(sprintKey) && walk && tory.Stamina > 1)
        {
            tory.Stamina -= 1;
        }
        else
        {
            if(tory.Stamina < 500)
                tory.Stamina += .25f;
        }
        if(Input.GetKeyDown(attack) && tory.Stamina > 10)
        {
            tory.Stamina -= 10;
        }
        //Debug.Log(tory.Stamina);
    }

	void Move(){
		direction = Input.GetAxis("Horizontal");
		
		if(direction >= .2 || direction <= -.2)
		{
			walk = true;
		}
		else
		{
			walk = false;
		}
	}

    void CheckforInput(){

		#region Setup Information for Input Checks
        Move();
		tory.GroundCheck();

		#endregion

        if(Input.GetKeyDown(pauseKey))
        {
            //pauseCode
        }

        if(Input.GetKeyDown(jumpKey))
        {
			tory.GroundCheck();
			tory.Jump();
        }

        if(Input.GetKeyDown(sprintKey) && walk && tory.Stamina > 0)
        {
            tory.Sprint(direction);
        }
        else if(walk)
        {
            tory.Walk(direction);
        }
        else
        {
            if(tory.IsGrounded)
            {
                tory.StopMoving();
            }
        }

        if(Input.GetKeyDown(attack))
        {
            tory.Attack();
        }

        if(Input.GetKeyDown(interactKey))
        {
            tory.Interact();
        }
        if(Input.GetKeyDown(switchWeapon))
        {
            tory.switchWeapon();
        }
        if(Input.GetKeyDown(KeyCode.J))
        {
            tory.DrinkWater();
        }

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(tory.FlashColor());
            tory.TakeDamage(10);
        }
    }
}
