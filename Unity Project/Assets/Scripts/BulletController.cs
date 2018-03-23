using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float bulletSpeed = 8f;
    public GameObject zombie;
    public GameObject bandit;


	// Use this for initialization
	void Start () {
       
        GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, GetComponent<Rigidbody2D>().velocity.x);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("BulletTriggerEnter");

        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<ZombieControllerScript>().TakeDamage();

        }
    }

}
