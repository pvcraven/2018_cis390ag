using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : IRangedWeaponInterface
{
    #region Properties

    public int AmmunitionSpeed
    {
        get { return ammunitionSpeed; }
        set { ammunitionSpeed = value; }
    }
    public int AmmunitionDamage
    {
        get { return ammunitionDamage; }
        set { ammunitionDamage = value; }
    }

    public int FireRatePerSecond
    {
        get { return fireRatePerSecond; }
        set { fireRatePerSecond = value; }
    }

    #endregion

    #region Variables
    private int ammunitionSpeed = 10;
    private int ammunitionDamage = 50;
    private int fireRatePerSecond = 2;
    #endregion

    #region Constructor

    public Gun(string gun)
    {
        switch(gun)
        {
            case "pistol":
                this.ammunitionDamage = 25;
                this.ammunitionSpeed = 35;
                this.fireRatePerSecond = 1;
                break;

            case "assaultRifle":
                this.ammunitionDamage = 35;
                this.ammunitionSpeed = 50;
                this.fireRatePerSecond = 3;
                break;
        }
    }

    #endregion

    #region Methods
    public void RangedTrigger()
    {
        
    }

    #endregion
}
