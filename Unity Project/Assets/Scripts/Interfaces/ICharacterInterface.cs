using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterInterface {

	//Properties
	int Health {get; set;}
	int Strength { get; set;}
	int Speed {get; set;}

	//Methods
	void TakeDamage(int damage);

	void Walk(float direction, float paceDistance = 0);

	void CheckDirection(float direction);

	void FlipDirection();

	void FlashColor();

}
