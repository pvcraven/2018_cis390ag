using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ICharacterInterface {


	#region Properties
    public int Health{
		get {return health;}
		set {health = value;}}
    public int Strength {
		get {return strength;}
		set {strength = value;}}
	public int Speed {
		get{return speed;}
		set{speed = value;}}
	public bool IsGrounded{
		get{return Physics2D.Linecast(this.player.GetComponent<PlayerController>().startOnPlayer.position, this.player.GetComponent<PlayerController>().endOnGround.position, 1 << LayerMask.NameToLayer("Ground")); }
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
    
	#endregion
	
	#region Variables
	private int health = 100;
	private int strength = 10;
	private int speed = 2;
	private bool isGrounded = false;
	private int jumpForce = 0;
	private int fallMultiplier = 3;
	private int lowJumpMultiplier = 2;
	private bool facingRight = true;

    #endregion

    #region Components
    public GameObject player;

    public Transform startOnPlayer, endOnGround;

    #endregion

    #region Contrustor
    public Player(GameObject player){

		this.Health = 100;
		this.Strength = 0;
		this.Speed = 5;
		this.IsGrounded = true;
        this.JumpForce = 5;
		this.FallMultiplier = 3;
		this.LowJumpMultiplier = 2;
		this.FacingRight = true;
        this.player = player;
    }

	#endregion
	
	#region Methods 
	public void Jump(){
		if(this.IsGrounded)
		{
            Debug.Log("Jump");

            if (player.GetComponent<Rigidbody2D>().velocity.y < 0)
            {
                player.GetComponent<Rigidbody2D>().velocity += Vector2.up * this.JumpForce * Physics2D.gravity.y * (this.FallMultiplier -= 1) * Time.deltaTime;
            }
            else if (player.GetComponent<Rigidbody2D>().velocity.y > 0 && !Input.GetButton("Jump"))
            {
                player.GetComponent<Rigidbody2D>().velocity += Vector2.up * this.JumpForce * Physics2D.gravity.y * (this.LowJumpMultiplier -= 1) * Time.deltaTime;
            }
           
            GroundCheckHandler();
		}}

    public void Walk(float direction) {
		CheckDirection(direction);
		player.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * this.speed, player.GetComponent<Rigidbody2D>().velocity.y);}

	public void Sprint(float direction){
		CheckDirection(direction);
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(direction * speed * 1.5f, player.GetComponent<Rigidbody2D>().velocity.y);}

	public void MeleeAttack(){
		Debug.Log("MeleeAttack");}

	public void RangedAttack(GameObject rangedObject, Transform rangedSpawn){
		Debug.Log("RangedAttack");
		GameObject.Instantiate(rangedObject, rangedSpawn.position, Quaternion.identity);

	}

	public void Interact(){
		Debug.Log("Interact");}

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

    private bool GroundCheckHandler(){
        
        if (this.IsGrounded)
        {
            player.GetComponent<Animator>().SetBool("OnGround", IsGrounded);
            player.GetComponent<Animator>().SetFloat("vSpeed", 0);
        }
        else
        {
            player.GetComponent<Animator>().SetFloat("vSpeed", player.GetComponent<Rigidbody2D>().velocity.y);
            player.GetComponent<Animator>().SetBool("OnGround", IsGrounded);
            player.GetComponent<Animator>().Play("Jump/Fall");
        }
        Debug.Log(this.IsGrounded);

        return this.IsGrounded;
    }

    public IEnumerator FlashColor(Color color) {
		//spriteRend is a SpriteRenderer
        var normalColor = player.GetComponent<SpriteRenderer>().material.color;

        player.GetComponent<SpriteRenderer>().material.color = color;
        yield return new WaitForSeconds(0.25F);

        player.GetComponent<SpriteRenderer>().material.color = color;
        yield return new WaitForSeconds(0.1F);}

	#endregion
}
