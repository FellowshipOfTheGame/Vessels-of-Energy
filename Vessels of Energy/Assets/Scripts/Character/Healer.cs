using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Character {
    public const int HEAL_COST = 3;
    public const int HEAL_RANGE = 1;

    void Start() {

        if(this.stats != null){
            this.stats.calculateStats();
        }
        else{
            Debug.Log("Character don't have stats!!!");
        }
        this.HP = stats.maxHP;
        this.stamina = stats.maxStamina;
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
            int healing = this.rollDices(this.stats.willpower+4);

            target.HP += healing;
            if(target.HP > target.stats.maxHP) target.HP = target.stats.maxHP;
        }
        return 0;
    }
}
