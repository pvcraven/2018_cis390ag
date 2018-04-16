using System;
using UnityEngine;

namespace Classes
{
	public class Weapon
	{
		#region Properties
		public int Damage
		{
			get { return damage; }
			set { damage = value; }
		}

		public int Durability
		{
			get { return durability; }
			set { durability = value; }
		}

		public bool IsRanged
		{
			get { return isRanged; }
			set { isRanged = value; }
		}

		public bool IsInMeleeRange
		{
			get { return isInMeleeRange; }
			set { isInMeleeRange = value; }
		}

		public float Range
		{
			get { return range; }
			set { range = value; }
		}
		#endregion
		
		#region Fields
		protected int damage;
		protected int durability;
		protected bool isRanged;
		protected bool isInMeleeRange;
		protected float range;
		public Player player;
		#endregion
		
		#region Default variables
		public static readonly float DEFAULT_RANGE = 0f;
		public static readonly int DEFAULT_DAMAGE = 100;
		public static readonly int DEFAULT_DURABILITY = 100;
		public static readonly bool DEFAULT_IS_RANGED = false;
		public static readonly bool DEFAULT_IS_IN_MELEE_RANGE = false;
		#endregion

		#region Constructors
		public Weapon()
		{
			this.damage = DEFAULT_DAMAGE;
			this.durability = DEFAULT_DURABILITY;
			this.isRanged = DEFAULT_IS_RANGED;
			this.isInMeleeRange = DEFAULT_IS_IN_MELEE_RANGE;
			this.range = DEFAULT_RANGE;
		}
		
		public Weapon(int damage, int durability, bool isRanged, float range)
		{
			this.damage = damage;
			this.durability = durability;
			this.isRanged = isRanged;
			this.range = range;
		}
		#endregion

		#region Methods
		
		public void Attack(GameObject otherGameObject)
		{
			if (this.isRanged)
			{
				AttackRanged();
			}
			else
			{
				AttackMelee(otherGameObject);
			}
		}
		
		public void AttackMelee(GameObject otherGameObject)
		{
			// Fill in with other enemy situations when need be
			if (otherGameObject.CompareTag("Zombie"))
			{
				Zombie zombie = otherGameObject.GetComponent<ZombieController>().Z;
				zombie.TakeDamage(this.damage);
			}
			else if (otherGameObject.CompareTag("Bandit"))
			{
				BanditEnemyController banditEnemyController = otherGameObject.GetComponent<BanditEnemyController>();
				banditEnemyController.TakeDamage(this.damage);
			}

			this.durability--;
		}

		public void AttackRanged()
		{
			// Spawn a bullet. Will be updated once the Player class is cleaned up
			player.RangedAttack();
		}
		#endregion
	}
}