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

    public override void Action()
    {
        Debug.Log("Warrior Action");
        //If selected and target are from different teams
        if (target.team != GameManager.currentTeam)
        {
            //If warrior has enough stamina and target has health
            if (this.stamina >= ATTACK_COST && target.HP >= 0)
            {
                target.place.changeState("enemy");
                this.ShortDistanceAttack(target, this.weapon.minRange, this.weapon.maxRange);
            } else
                Debug.Log("Not enough stamina");
        }
        else
        {
            if (this.stamina >= ATTACK_COST)
            {
                this.ThrowAlly(target);
                return;
            }
            locked = false;
            target.Select();
        }


        if (this.stamina == 0) {
            locked = false;
        }
    }
    public int ShortDistanceAttack(Character target, int minRange, int maxRange)
    {
        Debug.Log("Warrior short Distance Attack");
        if (checkRange(minRange, maxRange))
        {
            attack.PrepareAttack(target);

        }
        //Debug.Log("Out of range");
        return 0;
    }

    public void ThrowAlly(Character target)
    {
        if (checkRange(1, 1))
        {
            bool canMove = false;
            //Find a way to select another character
            locked = false;
            target.Select();

            //target.OnTarget();
            int throwableDistance = stats.strength;

            // show throwable hexagons
            GridManager gridM = GridManager.instance;
            GridManager.Grid reach = gridM.getReach(target.place, throwableDistance);

            foreach (GridManager.GridPoint point in reach.grid)
            {
                if (point.hex.state.name == "reach")
                    point.hex.changeState("coop");
            }

           
            //target.OnMove(reach, destiny);
            this.stamina -= ATTACK_COST;
        }
    }

}
