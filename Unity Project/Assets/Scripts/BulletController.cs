using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float bulletSpeed = 8f;

    private Rigidbody2D rigidbody2D;
    private Collider2D collider2D;


	// Use this for initialization
	void Start () {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();

        rigidbody2D.velocity = new Vector2(bulletSpeed, rigidbody2D.velocity.x);
    }
}
