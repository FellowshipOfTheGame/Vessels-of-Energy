﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Character : Token
{
    public static Character target = null;
    public Puppet animator;
    [HideInInspector] public Attack attack;
    public const int ATTACK_COST = 3;
    public const int EVADE_COST  = 3;

    [Space(5)]

    public Weapon weapon;
    public Stats stats;
    public int HP;
    public int stamina;
    [Space(5)]

    public char team;
    [HideInInspector] public Color color;
    GridManager.Grid reach = null;
    GridManager.Grid range = null;

    void Awake() {
        if (animator) color = animator.GetComponent<ChangeColor>().GetColor(team);
    }

    // refill character's stamina
    public void refillStamina() {
        this.stamina = this.stats.maxStamina;
    }

    // calculate reach of a character
    public override void OnSelect()
    {
        animator.Select();
        //Debug.Log("Selected");
        //If selected character has no stamina or is from the other team
        if (this.stamina == 0 || team != GameManager.currentTeam) return;

        GridManager gridM = GridManager.instance;

        reach = gridM.getReach(place, stamina, "token", "enemy", "ally");

        foreach (GridManager.GridPoint point in reach.grid)
        {
            point.hex.changeState("reach");
        }

        reach = gridM.getReach(place, this.stamina);
        foreach (GridManager.GridPoint point in reach.grid) {
            Character c = (Character)point.hex.token;
            if (point.hex.state.name == "token" && c.team != this.team)
                point.hex.changeState("enemy");
            else if (point.hex.state.name == "token" && c.team == this.team)
                point.hex.changeState("ally");
        }

        locked = true;
    }

    //Get the target of an action
    public override void TargetSelect()
    {
        if (selected != this) animator.Target();
        target = this;
    }

    public override void OnCancelSelect()
    {
        //animator.Unselect();
        if (reach == null) return;

        foreach (GridManager.GridPoint point in reach.grid)
        {
            if (point.hex.state.name == "reach")
                point.hex.changeState("default");

            if (point.hex.state.name == "enemy")
                point.hex.changeState("token");

            if (point.hex.state.name == "ally")
                point.hex.changeState("token");

            if (point.hex.state.name == "coop")
                point.hex.changeState("default");
        }
        Debug.Log("Cancelling select on " + name);

        locked = false;
        target = null;
    }

    public override void OnMove(GridManager.Grid path, HexGrid destiny)
    {
        base.OnMove(path, destiny);
        if (reach == null) return;

        int qtd = path.grid.Count;

        foreach (GridManager.GridPoint point in reach.grid)
        {
            if (point.hex.state.name == "enemy")
                point.hex.changeState("token");

            if (point.hex.state.name == "ally")
                point.hex.changeState("token");

            if (point.hex.state.name == "coop")
                point.hex.changeState("reach");

            if (point.hex.state.name == "reach")
                point.hex.changeState("default");
        }

        this.stamina -= qtd;
        GridManager gridM = GridManager.instance;

        reach = gridM.getReach(destiny, stamina, "token", "enemy", "ally");
        foreach (GridManager.GridPoint point in reach.grid)
        {
            point.hex.changeState("reach");
        }

        reach = gridM.getReach(destiny, this.stamina);
        foreach (GridManager.GridPoint point in reach.grid) {
            Character c = (Character)point.hex.token;
            if (point.hex.state.name == "token" && c.team != this.team)
                point.hex.changeState("enemy");
            else if (point.hex.state.name == "token" && c.team == this.team)
                point.hex.changeState("ally");
        }

        if (this.stamina == 0) {
            locked = false;
        }
    }

    //After using using an Action, updates reach for selected
    public override void updateReach()
    {
        if (reach == null) return;
        foreach (GridManager.GridPoint point in reach.grid)
        {
            if (point.distance > this.stamina)
            {
                if (point.hex.state.name == "enemy")
                    point.hex.changeState("token");

                if (point.hex.state.name == "ally")
                    point.hex.changeState("token");

                if (point.hex.state.name == "coop")
                    point.hex.changeState("reach");

                if (point.hex.state.name == "reach")
                    point.hex.changeState("default");
            }
        }
        Debug.Log("Reach " + reach.grid.Count.ToString() + " Updated on " + name);
    }

    //Check if ability can be used based on its min and max range
    public bool checkRange(int minDistance, int maxDistance)
    {
        GridManager gridM = GridManager.instance;

        range = gridM.getReach(place, minDistance, maxDistance);

        foreach (GridManager.GridPoint point in range.grid)
        {
            if (point.hex.token == target)
                return true;
        }
        return false;
    }

    //Rolls two dices and return their sum
    public int rollDices(int dice1, int dice2 = 0)
    {
        int value1 = Random.Range(1, dice1 + 1);
        if (dice2 == 0)
        {
            Debug.Log(value1);
            return value1;
        }
        int value2 = Random.Range(1, dice2 + 1);
        Debug.Log(value1 + value2);
        return value1 + value2;
    }

    //Checks if character evades an Attack
    public bool evade(int precision, int evasion)
    {
        if (precision >= evasion)
            return false;
        return true;
    }
}
