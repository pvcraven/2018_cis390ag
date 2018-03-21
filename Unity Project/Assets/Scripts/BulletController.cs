using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float bulletSpeed = 8f;

    private Rigidbody2D rb2D;
    private Collider2D coll2D;
    public GameObject zombie;
    public GameObject bandit;


	// Use this for initialization
	void Start () {
        rb2D = GetComponent<Rigidbody2D>();
        coll2D = GetComponent<Collider2D>();

        rb2D.velocity = new Vector2(bulletSpeed, rb2D.velocity.x);
    }

    void FixedUpdate()
    {
        DetectCollision();
    }


    void DetectCollision()
    {
        if (coll2D.IsTouching(zombie.GetComponent<Collider2D>()))
        {
            zombie.GetComponent<ZombieControllerScript>().health -= 50f;
            Destroy(rb2D.gameObject);
        }
    }
}
