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

	private bool jump;
	private bool walk;
	private bool sprint;
	private bool melee;
	private bool ranged;
	private bool interact;
	private bool pause;

    public Player tory = new Player();

    private float direction = 0;

	void Start()
	{
        jump = Input.GetKeyDown(jumpKey);
		sprint = Input.GetKeyDown(sprintKey) && walk;
		melee = Input.GetKeyDown(meleeKey);
		ranged = Input.GetKeyDown(rangedKey);
		interact = Input.GetKeyDown(interactKey);
		pause = Input.GetKeyDown(pauseKey);
	}

	void Update()
	{
		CheckforInput();
	}

	void Move()
	{
		direction = Input.GetAxis("Horizontal");
		
		if(direction >= .01 || direction <= -.01)
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

        if (pause)
        {
            //pauseCode
        }

        if (jump)
        {
            tory.Jump();
        }

        if (sprint)
        {
            tory.Sprint(direction);
        }
        else if (walk)
        {
            tory.Walk(direction);
        }

        if (melee)
        {
            tory.MeleeAttack();
        }
        else if (ranged)
        {
            tory.RangedAttack();
        }

        if (interact)
        {
            tory.Interact();
        }

    }
}
