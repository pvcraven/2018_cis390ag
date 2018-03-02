using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass {

    static int health;
    static int  stamina;

    static double attack = System.Math.Pow(2.0, (stamina + health)/10);
}
