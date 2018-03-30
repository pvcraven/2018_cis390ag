using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangedWeaponInterface{


    int AmmunitionSpeed { get; set; }
    int AmmunitionDamage { get; set; }
    int FireRatePerSecond { get; set; }
    void RangedTrigger();

}
