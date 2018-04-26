using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public Transform StartOnPlayer, EndOnGround;
    public Player tory;
    public GameObject statusBar;

    private SpriteRenderer spriteRend;
    private float direction = 0;

    //private float attackDelay = 20;
    //private bool updatedDelay = false;
    //private float attackCooldown = -1;
    //private bool animationDelay = false;

    private bool step = true;
    private bool sprintKeyDown = false;
    public AudioClip drinksound;
    public AudioClip pickupSound;
    public AudioClip gunshotSound;
	public AudioClip knifeSwipe;
    public AudioClip[] walkAudio;
	public AudioClip jumpSound;
	public AudioClip gameOverMusic;
	public float soundEffectVolumeLevel = 0.10f;

    void Start() {

        tory = new Player(player);

        audioSource = GetComponent<AudioSource>();
	}

	void Update(){

        //Why do we have a tory.Dead? It's redundant. If his health is <= 0 he is already dead.
        if (tory.Health <= 0)
        {
			tory.Die ();

            AudioSource bgMusic = GameObject.Find("Background Music").GetComponent<AudioSource>();
            bgMusic.clip = gameOverMusic;
            bgMusic.Play();
        }
	    
        //if (attackCooldown >= 0)
        //{
        //    attackCooldown--;
        //}
        //if (animationDelay)
        //{
        //    updatedDelay = MeleeAnimationDelay(animationDelay);
        //}
        //animationDelay = updatedDelay;

        if(tory.Dead == false)
            CheckforInput();

        if (Input.GetKey(sprintKey) && walk && tory.Stamina > 1)
        {
            tory.AdjustStamina(-1);

        }
        else
        {
            if(tory.Stamina < 500)
                tory.AdjustStamina(0.25f);
        }
        if(Input.GetKeyDown(attack) && tory.Stamina > 10)
        {
            tory.AdjustStamina(-10);
        }

        tory.FlashColor(); // Done every frame now to prevent staying on red
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
			audioSource.clip = jumpSound;
			audioSource.Play ();
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
            if(tory.IsGrounded && step == true)
            {
                StepSound();
                StartCoroutine (StepWait (audioSource.clip.length / 1.5f));
            }
        }
        else if(walk)
        {
            tory.Walk(direction);
            if(tory.IsGrounded && step == true)
            {
                StepSound();
                StartCoroutine(StepWait(audioSource.clip.length));
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
			if (tory.CurrentAttackType == "ranged") {
				tory.Attack (gunshotSound);
			} 
			else {
				tory.Attack (knifeSwipe);
			}
        }
        if (Input.GetKeyDown(interactKey))
        {
            Destroy(tory.Interact(pickupSound));
        }
        if (Input.GetKeyDown(switchWeapon))
        {
            tory.SwitchWeapon();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
	    tory.Weapon.IsInMeleeRange = true;
	    
        //Why Enemy? Do we have objects with this tag?
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Zombie") || 
			other.gameObject.CompareTag("Bandit") || other.gameObject.CompareTag("Spike"))
        {
            tory.color_flash_timer = 0.25f;
            tory.TakeDamage(7);
        }
        
        if (other.gameObject.CompareTag("Stone"))
        {
			GroundSound(0);
        }

        else if (other.gameObject.CompareTag("Dirt"))
        {
			GroundSound(1);
        }

        else if (other.gameObject.CompareTag("Grass"))
        {
			GroundSound(2);

        }

        if (other.gameObject.CompareTag("KillBlock"))
        {
            tory.TakeDamage(100);
        }
    }

	private void GroundSound(int groundType)
	{
		audioSource.clip = walkAudio[groundType];
		audioSource.volume = soundEffectVolumeLevel;
	}

	private void OnCollisionExit(Collision other)
	{
		tory.Weapon.IsInMeleeRange = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Equals("End Level Trigger"))
        {
            PlayerPrefs.SetInt("PreviousScene", SceneManager.GetActiveScene().buildIndex);

            SceneManager.LoadScene("TransitionSelection");
        }
    }

    #region Sound Code
    void StepSound()
    {
        audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
        audioSource.Play();
    }

    IEnumerator StepWait(float delay)
    {
        step = false;
        yield return new WaitForSeconds(delay);
        step = true;
    }
    #endregion

    //Did you just add bool b so you could you the same method name? This is bad overloading. What does this do?
    //public bool MeleeAnimationDelay(bool b)
    //{
    //    tory.SetAnimationFalse();
    //    return false;
    //}

    //public void MeleeAnimationDelay()
    //{
    //    attackCooldown = attackDelay;
    //    animationDelay = true;
    //}
}
