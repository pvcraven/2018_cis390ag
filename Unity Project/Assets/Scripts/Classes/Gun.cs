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
    public float AmmunitionDamage
    {
        get { return ammunitionDamage; }
        set { ammunitionDamage = value; }
    }

    public int FireRate
    {
        get { return fireRate; }
        set { fireRate = value; }
    }

    #endregion

    #region Variables
    private int ammunitionSpeed = 10;
    private float ammunitionDamage = 50;
    private int fireRate = 2;
    #endregion

    #region Constructor

    public Gun(string gun)
    {

        switch(gun)
        {
            case "pistol":
                this.ammunitionDamage = 25;
                this.ammunitionSpeed = 35;

          
                break;

            case "assaultRifle":
                this.ammunitionDamage = 35;
                this.ammunitionSpeed = 50;
                break;
             
        }
        
    }

    #endregion

    #region Methods
    public void ShootGun()
    {


    }

    #endregion
}
