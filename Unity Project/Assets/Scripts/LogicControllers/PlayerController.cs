using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public KeyCode jumpKey = KeyCode.Space;
	public KeyCode sprintKey = KeyCode.LeftShift;
	public KeyCode meleeKey = KeyCode.F;
	public KeyCode rangedKey = KeyCode.E;
	public KeyCode interactKey = KeyCode.R;
	public KeyCode pauseKey = KeyCode.Escape; 

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

	void Start(){
        tory = new Player(player);
        gunShot = GetComponent<AudioSource>();
	}

	void Update(){
		CheckforInput();
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

        if(Input.GetKeyDown(sprintKey) && walk)
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

        if(Input.GetKeyDown(meleeKey))
        {
            tory.MeleeAttack();
        }
        else if(Input.GetKeyDown(rangedKey))
        {
            tory.RangedAttack();
            gunShot.Play();
        }

        if(Input.GetKeyDown(interactKey))
        {
            tory.Interact();
        }

    }

	void OnCollisionEnter()
	{

	}
}
