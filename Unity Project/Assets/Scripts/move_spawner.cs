using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_spawner : MonoBehaviour {

	public float acceleration;
  public float deceleration;
	public GameObject zombie_prefab;

	private GameObject player;
  private Rigidbody2D rigid_body;
	private Transform player_transform;
	private float randomized_acceleration;
  private Vector2 input;
	private bool grounded;
	private int number_spawned;

  public void Start() {
		rigid_body = GetComponent<Rigidbody2D>();

		player = GameObject.Find("Player");
		player_transform = player.transform;

		randomized_acceleration = acceleration + Random.Range(-5.0f, 5.0f);

		grounded = false;
  }

  public void Update() {

  }

  public void FixedUpdate() {
		Vector2 player_position = player_transform.position;
		Vector2 zombie_position = transform.position;

		if(grounded) {
			input.y = 20;
			spawn();
		} else input.y = 0;

		rigid_body.AddForce(input * randomized_acceleration * Time.deltaTime, ForceMode2D.Impulse);
  }

	private void spawn() {
		if(Time.realtimeSinceStartup*2 > number_spawned) {
			number_spawned++;
			GameObject Clone;
			Clone = Instantiate(zombie_prefab, transform.position, Quaternion.identity) as GameObject;
			Physics2D.IgnoreCollision(Clone.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		}
	}

	private void OnCollisionEnter2D(Collision2D the_collision) {
		if(the_collision.gameObject.name == "Grid")
			grounded = true;
		if(the_collision.gameObject.tag == "Bullet")
				Destroy(gameObject);
	}

	private void OnCollisionExit2D(Collision2D the_collision) {
		if(the_collision.gameObject.name == "Grid")
			grounded = false;
	}
}
