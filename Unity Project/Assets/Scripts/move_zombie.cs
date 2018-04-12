using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_zombie : MonoBehaviour {

	public float acceleration;
  public float deceleration;

	private GameObject player;
  private Rigidbody2D rigid_body;
	private Transform player_transform;
	private float randomized_acceleration;
  private Vector2 input;
	private bool grounded;

  public void Start() {
		rigid_body = GetComponent<Rigidbody2D>();

		player = GameObject.Find("Player");
		player_transform = player.transform;

    rigid_body.drag = deceleration;

		randomized_acceleration = acceleration + Random.Range(-5.0f, 5.0f);

		grounded = false;
  }

  public void Update() {

  }

  public void FixedUpdate() {
		Vector2 player_position = player_transform.position;
		Vector2 zombie_position = transform.position;

		if(player_position.x < zombie_position.x) input.x = -1;
		if(player_position.x > zombie_position.x) input.x = 1;

		if(grounded) input.y = 17;
		else input.y = 0;

		rigid_body.AddForce(input * randomized_acceleration * Time.deltaTime, ForceMode2D.Impulse);
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
