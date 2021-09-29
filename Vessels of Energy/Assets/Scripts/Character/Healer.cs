using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Character {
    public const int HEAL_COST = 2;
    public const int HEAL_RANGE = 1;

    void Start() {

        if(this.stats != null){
            this.stats.calculateStats();
        }
        else{
            Debug.Log("Character don't have stats!!!");
        }
        this.HP = stats.maxHP;
    }

    public override void Action(){
        //If selected and target are from different teams
        if (target.team != GameManager.currentTeam) {
            if (this.stamina >= ATTACK_COST && target.HP >= 0) {
                target.place.changeState("enemy");
                this.Attack(target, this.weapon.minRange, this.weapon.maxRange);
            } else
                Debug.Log("Not enough stamina");
        }

        //If selected and target are from the same team
        else if(target.team == GameManager.currentTeam){
            if(this.stamina >= HEAL_COST && target.HP != target.stats.maxHP)
                this.Heal(target, 0, HEAL_RANGE);
            else
                Debug.Log("Not enough stamina or target at full health");
        }

        else {
            locked = false;
            target.Select();
        }

        //If selected and target are from the same team
        /*else if(target.team == GameManager.currentTeam){
            if(this.stamina >= HEAL_COST && target.HP != target.stats.maxHP){

                //If target is in range of the Heal ability
                if (checkRange(0, HEAL_RANGE)){
                    Debug.Log("Healing ally!");
                    this.Heal(target);
                }
                else
                    Debug.Log("Heal cannot reach target");
            }
            else
                Debug.Log("Not enough stamina or ally has full health");
        }*/

        if(this.stamina == 0){
            locked = false;
        }
    }

    public int Attack(Character target, int minRange, int maxRange) {
        Debug.Log("Healer Attack");
        if (checkRange(minRange, maxRange)) {
            attack.PrepareAttack(target);
        }
        return 0;
    }

    //Restores health for target
    //Target can be an ally or self
    public int Heal(Character target, int minRange, int maxRange) {
        Debug.Log("Healing");
        if(checkRange(minRange, maxRange)) {
            this.stamina -= HEAL_COST;
            //TODO: Balance this ability with a multiplier (mult*willpower)
            int healing = this.stats.willpower;

            target.HP += healing;
            if(target.HP > target.stats.maxHP) target.HP = target.stats.maxHP;
        }
        return 0;
    }

    //Restores health for target
    //Target can be an ally or self
    /*public int Heal(Character target) {
        this.stamina -= HEAL_COST;
        //TODO: Balance this ability with a multiplier (mult*willpower)
        int healing = this.stats.willpower;
        target.HP += healing;
        Debug.Log("Healing:");
        Debug.Log(healing);
        //If health goes above max health
        if(target.HP > target.stats.maxHP) target.HP = target.stats.maxHP;

        return 0;
    }*/
}
