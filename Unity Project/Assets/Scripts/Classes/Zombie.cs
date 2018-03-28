using UnityEngine;

namespace Classes
{
    public class Zombie : Enemy
    {
        #region Variables
        
        #region Default values

        private const int DEFAULT_DAMAGE = 10;
        
        #endregion
        
        #endregion
        
        #region Constructors

        public Zombie(GameObject zombie) : base(zombie)
        {
            this.damage = DEFAULT_DAMAGE;
        }

        public Zombie(GameObject enemy, int health, int damage, float maxSpeed, bool isRanged, bool hasFoundPlayer, 
            bool isFacingLeft) 
            : base(enemy, health, damage, maxSpeed, isRanged, hasFoundPlayer, isFacingLeft)
        {
            
        }

        #endregion
    }
}