using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon", fileName = "New Weapon")]
public class Weapon : ScriptableObject {
    new public string name;
    public int minRange;
    public int maxRange;
    public int baseDamageDice;
    public char damagetype; // [p]hysical or [m]agic
}
