using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangedWeaponInterface{


    int AmmuntionSpeed { get; set; }
    int AmmunitionDamage { get; set; }
    int FireRate { get; set; }
    void FireTrigger();

}
