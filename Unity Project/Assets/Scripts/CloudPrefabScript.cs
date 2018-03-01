using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPrefabScript : MonoBehaviour {

	//height above camera center
	private float maxY;
	public float minY;

	public float minSpeed;
	public float maxSpeed;
	public float buffer;
	float speed;
	private Transform cameraTransform;
	float camWidth;

	void Start() {
		maxY = Camera.main.orthographicSize;
		cameraTransform = Camera.main.transform;
		camWidth = Camera.main.orthographicSize * Camera.main.aspect;

		speed = Random.Range (minSpeed, maxSpeed);
		transform.position = new Vector2 (cameraTransform.position.x + camWidth + buffer, Random.Range (cameraTransform.position.y + minY, cameraTransform.position.y + maxY));
	}

	void Update () {

		transform.Translate (-speed * Time.deltaTime, 0, 0);

		if (transform.position.x + buffer < cameraTransform.position.x - camWidth) {
			Destroy (gameObject);
			CloudManager.cloudCount -= 1;
		}
	}
}
