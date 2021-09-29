using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Stats", fileName = "New Stats")]
public class Stats : ScriptableObject
{
    public int dexterity;
    public int strength;
    public int vitality;
    public int intelligence;
    public int perception;
    public int willpower;

    [Space(5)]

    public int maxHP;
    public int maxStamina;
    public int evasion;
    public int defense;
    public int resistence;

    //Calculate stats for Character
    public void calculateStats(){
        this.maxHP = 10 + 2 * this.vitality + this.willpower;
        this.maxStamina = 10 + 2 * this.dexterity + this.intelligence;
        this.evasion = 4 + this.dexterity + this.perception;
        this.defense = 2 + this.vitality + (this.strength / 2);
        this.resistence = 2 + this.intelligence + (this.willpower / 2);
    }
}
