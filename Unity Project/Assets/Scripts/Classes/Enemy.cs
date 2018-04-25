using System.CodeDom;
using UnityEngine;

namespace Classes
{
    public class Enemy
    {
        #region Properties

        public int Health {
            get { return this.health; }
            set { this.health = value; }
        }

        public int Damage {
            get { return this.damage; }
            set { this.damage = value; }
        }

        public bool IsRanged {
            get { return this.isRanged; }
            set { this.isRanged = value; }
        }

        #endregion
        
        #region Variables

        protected int health;
        protected int damage;
        protected float maxSpeed;
        protected float currentSpeed;
        protected bool isRanged;
        protected bool hasFoundPlayer;
        protected bool isFacingLeft;
        
        #region Default values
        private const int DEFAULT_HEALTH = 100;
        private const int DEFAULT_DAMAGE = 10;
        private const float DEFAULT_MAX_SPEED = 10f;
        private const bool DEFAULT_IS_RANGED = false;
        private const bool DEFAULT_IS_AWARE_OF_PLAYER = false;
        private const bool DEFAULT_IS_FACING_LEFT = true;
        
        #endregion
        
        #endregion
        
        #region Components

        public GameObject enemy;
        
        #endregion
        
        #region Constructors
        
        public Enemy(GameObject enemy) {
            this.enemy = enemy;
            this.health = DEFAULT_HEALTH;
            this.damage = DEFAULT_DAMAGE;
            this.currentSpeed = 0f;
            this.maxSpeed = DEFAULT_MAX_SPEED;
            this.isRanged = DEFAULT_IS_RANGED;
            this.hasFoundPlayer = DEFAULT_IS_AWARE_OF_PLAYER;
            this.isFacingLeft = DEFAULT_IS_FACING_LEFT;
        }

        public Enemy(GameObject enemy, int health, int damage, float maxSpeed, bool isRanged) {
            this.enemy = enemy;
            this.health = health;
            this.currentSpeed = 0f;
            this.maxSpeed = maxSpeed;
            this.damage = damage;
            this.isRanged = isRanged;
            this.hasFoundPlayer = DEFAULT_IS_AWARE_OF_PLAYER;
            this.isFacingLeft = DEFAULT_IS_FACING_LEFT;

            InitializeComponents();
        }
        
        #endregion
        
        #region Methods

        private void InitializeComponents() {
        }

        public void Attack() {
            if (this.isRanged) {
                RangedAttack();
            } else {
                MeleeAttack();
            } 
        }

        private void RangedAttack() {
            
        }

        private void MeleeAttack() {
            
        }
        
        #endregion
    }
}