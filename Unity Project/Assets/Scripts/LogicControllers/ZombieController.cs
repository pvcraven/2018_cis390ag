using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour {

    public GameObject zombie;
    private Zombie z;
    public Transform sightStart, sightEnd;
    private System.Random rand = new System.Random();

    // Use this for initialization
    void Start () {
        z = new Zombie(zombie);
        
    }

    // Update is called once per frame
    void Update () {

        z.Walk(1, rand.Next(5, 20));

        if (z.Health <= 0)
        {
            Destroy(zombie);
        }
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            z.TakeDamage(50);
            StartCoroutine(z.FlashColor());
        }
    }

    public void TakeDamage(int damage)
    {
        z.TakeDamage(damage);
    }
    
    public Zombie Z
    {
        get { return z; }
        set { z = value; }
    }
}
