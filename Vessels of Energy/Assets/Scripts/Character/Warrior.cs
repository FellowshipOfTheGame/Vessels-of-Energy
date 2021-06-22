using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Character {
    /*private int maxRange = 1;
    private int minRange = 1;*/

    void Start() {

        if(this.stats != null){
            this.stats.calculateStats();
        }
        else{
            Debug.Log("Character don't have stats!!!");
        }
        this.HP = stats.maxHP;
    }

    public override void Action() {
        Debug.Log("Warrior Action");
        //If selected and target are from different teams
        if (target.team != GameManager.currentTeam) {
            //If warrior has enough stamina and target has health
            if (this.stamina >= ATTACK_COST && target.HP >= 0) {
                target.place.changeState("enemy");
                this.ShortDistanceAttack(target, this.weapon.minRange, this.weapon.maxRange);
            } else
                Debug.Log("Not enough stamina");
        } else {
            locked = false;
            target.Select();
        }


        if (this.stamina == 0) {
            locked = false;
        }
    }
    public int ShortDistanceAttack(Character target, int minRange, int maxRange) {
        Debug.Log("Warrior short Distance Attack");
        if (checkRange(minRange, maxRange)) {
            attack.PrepareAttack(target);

        }
        //Debug.Log("Out of range");
        return 0;
    }
}
