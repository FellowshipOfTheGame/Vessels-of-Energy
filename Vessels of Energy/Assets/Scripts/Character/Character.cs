using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Character : Token {
    public static Character target = null;
    public Puppet animator;
    [HideInInspector] public Attack attack;
    public const int ATTACK_COST = 3;
    public const int EVADE_COST = 3;



    [Space(5)]

    public Weapon weapon;
    public Stats stats;
    public int HP;
    public int stamina;
    public bool action;
    [Space(5)]

    public Team team;
    [HideInInspector] public Color color;
    GridManager.Grid reach = null, border = null;
    bool used = false;

    void Awake() {
        if (animator) color = team.color[0];
    }

    // calculate reach of a character
    public override void OnSelect() {
        if (used) {
            Unselect();
            return;
        }

        animator.Select();
        Debug.Log(Colored("Selected"));
        //If selected character has no stamina or is from the other team
        if (this.stamina == 0 || team != GameManager.currentTeam || this.action == false) return;
        scan();
        locked = true;
    }

    void scan() {
        GridManager gridM = GridManager.instance;

        reach = gridM.getReachWithBorder(place, stamina, true, out border, 1);
        foreach (GridManager.GridPoint point in reach.grid) point.hex.changeState("reach");

        foreach (GridManager.GridPoint point in border.grid) {
            Character c = (Character)point.hex.token;
            if (point.hex.getState() == "token" && c.team != this.team)
                point.hex.changeState("enemy");
            else if (point.hex.getState() == "token" && c.team == this.team)
                point.hex.changeState("ally");
        }
    }

    //Get the target of an action
    public override void TargetSelect() {
        if (selected != this) animator.Target();
        target = this;
    }

    public override void OnCancelSelect() {
        //animator.Unselect();
        updateReach(true);
        locked = false;
        target = null;
    }

    public override void OnMove(GridManager.Grid path, HexGrid destiny) {
        if (!isFrozen) {
            int qtd = path.grid.Count;
            this.stamina -= qtd;
            base.OnMove(path, destiny);
            updateReach(true);
        }
    }

    public override void OnFinishMove() {
        scan();
        if (this.stamina == 0) locked = false;
    }

    public override void OnFinishAction() {
        if (!used) {
            updateReach(false);
        }
    }
    public override void OnTurnStart() {
        this.stamina = this.stats.maxStamina;
        this.action = true;
        this.used = false;
    }

    public void FinishTurn() { used = true; }


    //After using using an Action, updates reach for selected
    public void updateReach(bool erase) {
        if (reach != null) {
            Debug.Log("updating reach...");
            foreach (GridManager.GridPoint point in reach.grid) {
                if (point.distance > this.stamina || erase) {
                    point.hex.changeState("default");
                } else {
                    point.hex.changeState("reach");
                }
            }
        }

        if (border != null) {
            foreach (GridManager.GridPoint point in border.grid) {
                if (point.distance > this.stamina || erase) {
                    if (point.hex.getState() == "enemy") point.hex.changeState("token");
                    if (point.hex.getState() == "ally") point.hex.changeState("token");
                }
            }
        }

        if (erase) {
            reach = null;
            border = null;
        }
    }

    //Check if ability can be used based on its min and max range
    public bool checkRange(int minDistance, int maxDistance) {
        GridManager gridM = GridManager.instance;
        GridManager.Grid range = null;

        range = gridM.getReach(place, minDistance, maxDistance, false);

        foreach (GridManager.GridPoint point in range.grid) {
            if (point.hex.token == target)
                return true;
        }
        return false;
    }

    //Rolls two dices and return their sum
    public int rollDices(int dice1, int dice2 = 0) {
        int value1 = Random.Range(1, dice1 + 1);
        if (dice2 == 0) {
            Debug.Log(value1);
            return value1;
        }
        int value2 = Random.Range(1, dice2 + 1);
        Debug.Log(value1 + value2);
        return value1 + value2;
    }

    public virtual void EnableAction() { }

    public string Colored(string text) {
        //Colored Debug.Log: Debug.Log("<color=#0000FF>colored text</color> normal text");
        string _prefix = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>";
        string _suffix = "</color>";
        return _prefix + name + ": " + _suffix + text;
    }
}
