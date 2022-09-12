using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Character {
    public const int HEAL_COST = 3;
    public const int HEAL_RANGE = 1;

    void Start() {

        if (this.stats != null) {
            this.stats.calculateStats();
        } else {
            Debug.Log("Character don't have stats!!!");
        }
        this.HP = stats.maxHP;
        this.stamina = stats.maxStamina;
        this.energy = 0;
    }

    public override void Action(Token target) {
        Character c = (Character)target;

        //If selected and target are from different teams
        if (c.team != GameManager.currentTeam) {
            if (this.stamina >= ATTACK_COST && c.HP >= 0) {
                target.place.changeState("enemy");
                this.Attack(c);
            } else
                Debug.Log("Not enough stamina");
        }

        //If selected and target are from the same team
        else if (c.team == GameManager.currentTeam) {
            if (this.stamina >= HEAL_COST && c.HP != c.stats.maxHP)
                this.Heal(c, 0, HEAL_RANGE);
            else
                Debug.Log("Not enough stamina or target at full health");
        } else {
            locked = false;
            target.Select();
        }

        if (this.stamina == 0) {
            locked = false;
        }
    }

    //Restores health for target
    //Target can be an ally or self
    public int Heal(Character target, int minRange, int maxRange) {
        Debug.Log("Healing");
        if (checkRange(minRange, maxRange, target.place)) {
            this.stamina -= HEAL_COST;
            this.energy = Mathf.Min(this.energy+1, this.stats.power);
            int healing = this.rollDices(this.stats.willpower + 4);

            target.HP = Mathf.Min(target.HP+healing, target.stats.maxHP);
        }
        return 0;
    }
}
