using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FloatingItem : MonoBehaviour
{
	float initialY;
	public float floatSpeed = 0.1f;

	void Start () 
	{
		this.initialY = this.transform.position.y;
	}

	void Update () 
	{
		float floating = (float)Math.Sin(Time.time) * floatSpeed;
		transform.position = new Vector3 (transform.position.x, initialY + floating, transform.position.z);
	}
}
