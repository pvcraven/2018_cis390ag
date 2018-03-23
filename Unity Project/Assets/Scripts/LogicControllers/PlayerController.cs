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

	private bool walk;

    public GameObject player;
    public Transform startOnPlayer, endOnGround;
    public Player tory;

    private float direction = 0;

	void Start()
	{
        tory = new Player(player);
	}

	void Update()
	{
		CheckforInput();
	}

	void Move()
	{
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

    void CheckforInput()
    {
        Move();

        if(Input.GetKeyDown(pauseKey))
        {
            //pauseCode
        }

        if(Input.GetKey(jumpKey))
        {
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
                tory.player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }
        }

        if(Input.GetKeyDown(meleeKey))
        {
            tory.MeleeAttack();
        }
        else if(Input.GetKeyDown(rangedKey))
        {
            tory.RangedAttack();
        }

        if(Input.GetKeyDown(interactKey))
        {
            tory.Interact();
        }

    }
}
