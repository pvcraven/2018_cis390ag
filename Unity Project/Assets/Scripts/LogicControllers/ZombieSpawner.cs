using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour {

    public List<GameObject> zombiePrefab;
    public Transform zombieSpawner;
    public int zombieNumber;
    public System.Random rand = new System.Random();

    // Update is called once per frame
    void Update ()
    {
        zombieNumber = rand.Next(0, 250);

        //If we add more animations to the other zombie we can have different kinds of zombies spawn

        if(zombieNumber == 0)
        {
            GameObject zombie = UnityEngine.Object.Instantiate(zombiePrefab[zombieNumber], zombieSpawner.position, Quaternion.identity);

            UnityEngine.Object.Destroy(zombie, 30.0f);
        }

        

    }
}
