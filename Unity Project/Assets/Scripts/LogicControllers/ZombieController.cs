using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour {

    public GameObject zombie;
    private Zombie z;
    public Transform sightStart, sightEnd;
    public Transform StartOnZombie, EndOnGround;
    private System.Random rand = new System.Random();
    private bool isTouchingAnotherZombie = false;
    public int jumpForce = 150;
    private int health = 20;

    // Use this for initialization
    void Start () {
        z = new Zombie(zombie);
        z.jumpForce = jumpForce; //This is hacky and should be a z.JumpForce where JumpForce is a property but we're running out of time so... oh well...?
        
    }

    // Update is called once per frame
    void Update () {

        z.Walk(1, rand.Next(25, 200));

        if(z.Health <= 0)
        {
            Destroy(zombie);
        }

        if(this.isTouchingAnotherZombie)
        {
            if (rand.Next(0, 2) == 1)
            {
                z.Jump();
            }
            else
            {
                z.FlipDirection();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            z.TakeDamage(50);
        }

        //Doesn't work YET! We have to redo the stab method first.
        if(other.gameObject.CompareTag("Player")) //&& other.gameObject.GetComponent<PlayerController>().Stabbing == true) <-- Adding this functionality -CW
        {
            z.TakeDamage(50);
        }

        if (other.gameObject.CompareTag("Zombie"))
        {
            isTouchingAnotherZombie = true;
        }
            
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        isTouchingAnotherZombie = false;
    }
    
    public Zombie Z
    {
        get { return z; }
        set { z = value; }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Destroy(zombie);
        }
    }
}
