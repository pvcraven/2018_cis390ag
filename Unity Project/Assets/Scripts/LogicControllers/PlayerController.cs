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

    private AudioSource audioSource;

	private bool walk;

    public GameObject player;
	public GameObject rangedAmmunition;
	public Transform rangedSpawner;
    public Transform startOnPlayer, endOnGround;
    public Player tory;
    public AudioClip[] walkAudio;

    private float direction = 0;
    private SpriteRenderer spriteRend;
    private bool step = true;
    private bool sprintKeyDown = false;

	void Start() {
        tory = new Player(player);
        audioSource = GetComponent<AudioSource>();
	}

	void Update(){

        if (tory.Health <= 0)
        {
			tory.Dead = true;
			tory.Die ();
        }

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

    void WalkSound()
    {
        audioSource.clip = walkAudio[1];
        audioSource.volume = 0.05f;
        audioSource.pitch = Random.Range(0.8f, 1f);
        audioSource.Play();
        StartCoroutine(WalkWait(audioSource.clip.length));
    }

    IEnumerator WalkWait(float delay)
    {
        step = false;
        yield return new WaitForSeconds(delay);
        step = true;
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
        
        if (Input.GetKeyDown(sprintKey))
        {
            sprintKeyDown = true;
        }
        if (Input.GetKeyUp(sprintKey))
        {
            sprintKeyDown = false;
        }

        if (sprintKeyDown && walk && tory.Stamina > 0)
        {
            tory.Sprint(direction);
        }
        else if(walk)
        {
            //Debug.Log(sprintKeyDown + ", " + walk + ", " + tory.Stamina);
            tory.Walk(direction);
            if(tory.IsGrounded && step == true)
            {
                WalkSound();
            }
        }
        else
        {
            if(tory.IsGrounded)
            {
                tory.StopMoving();
            }
        }

        if (Input.GetKeyDown(attack))
        {
            Debug.Log("Attack");
            tory.Attack();
        }
        if (Input.GetKeyDown(interactKey))
        {
            Destroy(tory.Interact());
        }
        if (Input.GetKeyDown(switchWeapon))
        {
            tory.switchWeapon();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(tory.FlashColor());
            tory.TakeDamage(10);
            Debug.Log("You're Taking Damage! Health: " + tory.Health);
        }
    }
}
