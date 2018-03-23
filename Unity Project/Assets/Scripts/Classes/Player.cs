using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacterInterface {


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
		get{return isGrounded;}
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
	private int health = 0;
	private int strength = 0;
	private int speed = 0;
	private bool isGrounded = false;
	private int jumpForce = 0;
	private int fallMultiplier = 0;
	private int lowJumpMultiplier = 0;
	private bool facingRight = true;

	#endregion

	#region Components
	private Rigidbody2D rb2D;
	private Collider2D coll2D;
	private Animator anim;
	private SpriteRenderer spriteRend;

	#endregion

	#region Contrustor
	public Player(){

		this.Health = 100;
		this.Strength = 0;
		this.Speed = 5;
		this.IsGrounded = true;
		this.FallMultiplier = 5;
		this.LowJumpMultiplier = 2;
		this.facingRight = true;

		rb2D = GetComponent<Rigidbody2D>();
		coll2D = GetComponent<Collider2D>();
		anim = GetComponent<Animator>();
		spriteRend = GetComponent<SpriteRenderer>();}

	#endregion
	
	#region Methods 
	public void Jump(){
		if(this.isGrounded)
		{
			rb2D.velocity = Vector2.up * jumpForce;
		}}
	public void Walk(float direction) {
		CheckDirection(direction);
		rb2D.velocity = new Vector2(direction * this.speed, rb2D.velocity.y);}

	public void Sprint(float direction){
		CheckDirection(direction);
		rb2D.velocity = new Vector2(direction * speed * 1.5f, rb2D.velocity.y);}

	public void MeleeAttack(){
		Debug.Log("MeleeAttack");}

	public void RangedAttack(){
		Debug.Log("RangedAttack");}

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
		facingRight = !facingRight;
		Vector2 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;}

	public void TakeDamage(int damage) {
		this.health = this.health - damage;}


	public IEnumerator FlashColor(Color color) {
		//spriteRend is a SpriteRenderer
        var normalColor = spriteRend.material.color;

        spriteRend.material.color = color;
        yield return new WaitForSeconds(0.25F);

        spriteRend.material.color = color;
        yield return new WaitForSeconds(0.1F);}

	#endregion
}
