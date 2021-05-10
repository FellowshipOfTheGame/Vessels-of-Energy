using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Character
{
    void Start()
    {
        //Stats still need to be balanced
        dexterity = 2;
        strength = 0;
        vitality = 0;
        intelligence = 2;
        perception = 0;
        willpower = 4;

        this.calculateStats();
    }

    //Restores health for target
    //Target can be an ally or self
    public int Heal(Character target)
    {
        this.stamina -= 2;
        //TODO: Balance this ability with a multiplier (mult*willpower)
        int healing = this.willpower;
        target.HP += healing;
        //If health goes above max health
        if(target.HP > target.maxHP) target.HP = target.maxHP;

        return healing;
    }

    public override void Action(){
        //If selected and target are from the same team
        if(target.team == GameManager.currentTeam){
            //If healer has enough stamina and target has less health than maxHealth
            if(this.stamina >= 2 && target.HP != target.maxHP){
                //If target is in range of the Heal ability
                //Range of Heal is [0, 1]
                if (checkRange(0, 1)){
                    Debug.Log("Healing ally!");
                    this.Heal(target);
                }
                else
                    Debug.Log("Heal cannot reach target");
            }
            else
                Debug.Log("Not enough stamina or ally has full health");
        }
        else{ //selected and target are from different teams
            Debug.Log("Attacking target...");
        }

        if(stamina == 0){
            locked = false;
        }
    }
}
