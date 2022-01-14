using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artificer : Character {
    public const int OVERWATCH_COST = 2;
    public const int OVERWATCH_MIN_RANGE = 1;
    public const int OVERWATCH_MAX_RANGE = 5;

    public GridManager.Grid overwatchArea;

    List<HexGridEffect> overwatch = null;

    void Start() {

        if (this.stats != null) {
            this.stats.calculateStats();
        } else {
            Debug.Log("Character don't have stats!!!");
        }
        this.HP = stats.maxHP;
        this.stamina = stats.maxStamina;
    }

    public override void Action() {
        //If selected and target are from different teams
        if (target.team != team) {
            if (this.stamina >= ATTACK_COST && target.HP >= 0) {
                target.place.changeState("enemy");
                this.Attack(target, this.weapon.minRange, this.weapon.maxRange);
            } else
                Debug.Log("Not enough stamina");
        }

        //If selected and target are from the same team
        else if (target.team == team) {
            if (this.stamina >= OVERWATCH_COST) {
                this.Overwatch(target.place, OVERWATCH_MIN_RANGE, OVERWATCH_MAX_RANGE);
                Debug.Log("Overwatch used...");
            } else
                Debug.Log("Not Enough Stamina");
        } else {
            locked = false;
            target.Select();
        }

        if (this.stamina == 0) {
            locked = false;
        }
    }

    public int Attack(Character target, int minRange, int maxRange) {
        Debug.Log("Artificer Attack");
        if (checkRange(minRange, maxRange)) {
            attack.PrepareAttack(target);
        }
        return 0;
    }

    public int Overwatch(HexGrid target, int minRange, int maxRange) {
        Debug.Log(Colored("Overwatching Grid"));
        if (checkRange(minRange, maxRange)) {
            this.stamina -= OVERWATCH_COST;
            this.action = false;

            GridManager gridM = GridManager.instance;
            GridManager.Grid reach = gridM.getReach(target, 1, false);
            overwatchArea = reach;
            overwatch = new List<HexGridEffect>();
            foreach (GridManager.GridPoint point in reach.grid) {
                if (point.hex.getState() != "token") {
                    overwatch.Add(point.hex.addEffect("guard"));
                }
            }

            this.FinishTurn();
        }

        return 0;
    }

    public void cancelOverwatch() {/*
        foreach (GridManager.GridPoint point in overwatchArea.grid) {
            if (point.hex.getState() == "guard")
                point.hex.changeState("default");
        }*/

        foreach (HexGridEffect spot in overwatch) {
            spot.Cancel();
        }
    }

    public override void EnableAction() {
        this.action = true;
        this.stamina += OVERWATCH_COST;
        this.cancelOverwatch();
        Debug.Log("Enabled Artificer");
    }
}
