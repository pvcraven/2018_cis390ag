using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangedWeaponInterface{


    int AmmuntionSpeed { get; set; }
    int AmmunitionDamage { get; set; }
    int FireRate { get; set; }

    void OnTriggerEnter2D(Collider2D other);

    void OnTriggerExit2D(Collider2D other);
}
