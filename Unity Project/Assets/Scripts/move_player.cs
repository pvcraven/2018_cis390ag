using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_player : MonoBehaviour {

	public float acceleration;
	public float jump_force;
	public GameObject boom_prefab;

  private Rigidbody2D rigid_body;
  private Vector2 input;
	private bool grounded;

  public void Start() {
    rigid_body = GetComponent<Rigidbody2D>();

		grounded = false;
  }

  public void Update() {
    input.x = Input.GetAxis("Horizontal");
		if ((Input.GetKeyDown("up") || Input.GetKeyDown("w")) && grounded) input.y = jump_force;
		else input.y = 0;
  }

  public void FixedUpdate() {
    rigid_body.AddForce(input * acceleration * Time.deltaTime, ForceMode2D.Impulse);

		if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) )
			launch_boom();
  }

	private void launch_boom() {
		Vector2 small_jump = new Vector2(0,1);
		rigid_body.AddTorque(10000);

		instantiate_boom();
	}

	private float angle_between_vectors(Vector2 vec1, Vector2 vec2) {
		return Mathf.Atan2(vec1.y - vec2.y, vec1.x - vec2.x) * Mathf.Rad2Deg;
  }

	Vector2 get_vector_from_angle(float angle) {
		float rad = angle * Mathf.Deg2Rad;
		return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
	}

	private void instantiate_boom() {
		Vector3 mouse_3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 mouse_pos = new Vector2(mouse_3D.x, mouse_3D.y);
		float angle_of_fire = angle_between_vectors(transform.position, mouse_pos)-180;
		Vector2 boom_trajectory = get_vector_from_angle(angle_of_fire);

		GameObject Clone;
		Clone = Instantiate(boom_prefab, transform.position, Quaternion.identity) as GameObject;
		Physics2D.IgnoreCollision(Clone.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		Clone.GetComponent<Rigidbody2D>().AddForce(boom_trajectory * 100);
		Destroy(Clone,3);
	}

	private void OnCollisionEnter2D(Collision2D the_collision) {
		grounded = true;
	}

	private void OnCollisionExit2D(Collision2D the_collision) {
		grounded = false;
	}
}
