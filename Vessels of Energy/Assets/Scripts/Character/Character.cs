using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Character : Token {
    public static Character target = null;
    public Puppet animator;
    [HideInInspector] public Attack attack;
    public const int ATTACK_COST = 3;

    [Space(5)]

    public int dexterity;
    public int strength;
    public int vitality;
    public int intelligence;
    public int perception;
    public int willpower;
    [Space(5)]

    public int HP;
    public int maxHP;
    public int stamina = 0;
    public int maxStamina = 10; //temporary value for test
    public int evasion;
    public int defense;
    public int resistence;
    [Space(5)]

    public char team;
    GridManager.Grid reach = null;
    GridManager.Grid range = null;

    // reset character's stamina
    public void resetStamina() {
        stamina = maxStamina;
    }

    //Calculate stats values for Character
    public void calculateStats() {
        maxHP = 10 + 2 * vitality + willpower;
        HP = maxHP;
        maxStamina = 10 + 2 * dexterity + intelligence;
        evasion = 4 + dexterity + perception;
        defense = 2 + vitality + (strength / 2);
        resistence = 2 + intelligence + (willpower / 2);
    }

    // calculate reach of a character
    public override void OnSelect() {
        animator.Select();
        //Debug.Log("Selected");
        //If selected character has no stamina or is from the other team
        if (stamina == 0 || team != GameManager.currentTeam) return;

        GridManager gridM = GridManager.instance;

        reach = gridM.getReach(place, stamina, "token", "enemy");

        foreach (GridManager.GridPoint point in reach.grid) {
            point.hex.changeState("reach");
        }

        reach = gridM.getReach(place, stamina);
        foreach (GridManager.GridPoint point in reach.grid) {
            Character c = (Character)point.hex.token;
            if (point.hex.state.name == "token" && c.team != this.team)
                point.hex.changeState("enemy");
        }

        locked = true;
    }

    //Get the target of an action
    public override void TargetSelect() {
        if (selected != this) animator.Target();
        target = this;
    }

    public override void OnCancelSelect() {
        //animator.Unselect();
        if (reach == null) return;

        foreach (GridManager.GridPoint point in reach.grid) {
            if (point.hex.state.name == "reach")
                point.hex.changeState("default");
            if (point.hex.state.name == "enemy")
                point.hex.changeState("token");
        }

        locked = false;
    }

    public override void OnMove(GridManager.Grid path, HexGrid destiny) {
        base.OnMove(path, destiny);
        if (reach == null) return;

        int qtd = path.grid.Count;

        foreach (GridManager.GridPoint point in reach.grid) {
            if (point.hex.state.name == "enemy")
                point.hex.changeState("token");

            if (point.hex.state.name == "reach")
                point.hex.changeState("default");
        }

        stamina -= qtd;
        GridManager gridM = GridManager.instance;
        reach = gridM.getReach(destiny, stamina, "token", "enemy");

        foreach (GridManager.GridPoint point in reach.grid) {
            point.hex.changeState("reach");
        }

        reach = gridM.getReach(destiny, stamina);
        foreach (GridManager.GridPoint point in reach.grid) {
            Character c = (Character)point.hex.token;
            if (point.hex.state.name == "token" && c.team != this.team)
                point.hex.changeState("enemy");
        }

        if (stamina == 0) {
            locked = false;
        }
    }

    //After using using an Action, updates reach for selected
    public override void updateReach() {
        if ( reach == null ) return;
        foreach (GridManager.GridPoint point in reach.grid) {
            if (point.distance > this.stamina) {
                if ( point.hex.state.name == "enemy" )
                    point.hex.changeState("token");

                if ( point.hex.state.name == "reach" )
                    point.hex.changeState("default");
            }
        }
    }

    //Check if ability can be used based on its min and max range
    public bool checkRange(int minDistance, int maxDistance) {
        GridManager gridM = GridManager.instance;

        range = gridM.getReach(place, minDistance, maxDistance);

        foreach (GridManager.GridPoint point in range.grid) {
            if (point.hex.token == target)
                return true;
        }
        return false;
    }

    //Rolls two dices and return their sum
    public int rollDices(int dice1, int dice2 = 0){
        int value1 = Random.Range(1, dice1+1);
        if(dice2 == 0){
            Debug.Log(value1);
            return value1;
        }
        int value2 = Random.Range(1, dice2+1);
        Debug.Log(value1 + value2);
        return value1 + value2;
    }

    //Checks if character evades an Attack
    public bool evade(int precision, int evasion){
        if(precision >= evasion)
            return false;
        return true;
    }
}
