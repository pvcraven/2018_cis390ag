using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float bulletSpeed = 8f;


	// Use this for initialization
	void Start () {
       
        GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, GetComponent<Rigidbody2D>().velocity.x);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("BulletTriggerEnter");
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<ZombieControllerScript>().TakeDamage();
			Destroy(gameObject);
        }
    }

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    Destroy(gameObject);
    //}

}
