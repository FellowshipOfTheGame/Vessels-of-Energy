using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu (menuName = "Stats", fileName = "New Stats")]
public class Stats : ScriptableObject
{
    public int strength;
    public int dexterity;
    public int vitality;
    public int intelligence;
    public int perception;
    public int willpower;
    public int luck;

    [Space(5)]

    public int maxHP;
    public int maxStamina;
    public int evasion;
    public int defense;
    public int resistence;
    public int power;

    //Calculate stats for Character
    public void calculateStats(){
        this.maxHP = 20 + 4 * this.vitality + this.luck;
        this.maxStamina = 6 + 2 * this.dexterity + this.willpower;
        this.evasion = 4 + this.dexterity + this.perception;
        this.defense = 4 + System.Math.Max(this.vitality, this.strength);
        this.resistence = 4 + System.Math.Max(this.intelligence, this.willpower);
        this.power = 6 + this.strength + this.intelligence + this.luck;
    }
}
