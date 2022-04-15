using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Character {
    const int THROW_COST = 3;

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
        if (c.team == team && this.stamina >= THROW_COST) {
            ThrowAlly(c);
        }
    }

    public void ThrowAlly(Character target) {
        Debug.Log(Colored("Throw!"));

        if (checkRange(1, 1, target.place) && target.canBeMoved) {
            updateReach(true);

            // If Artificer used Overwatch, cancels ability and let him move again
            if (!target.action)
                target.EnableAction();

            //target.OnTarget();
            int throwableDistance = 2 + stats.strength;

            // show throwable hexagons
            GridManager gridM = GridManager.instance;
            GridManager.Grid reach = gridM.getReach(target.place, throwableDistance, false);
            foreach (GridManager.GridPoint point in reach.grid) {
                string state = point.hex.getState();
                if (state == "reach" || state == "default")
                    point.hex.changeState("coop");
            }

            DelayedAction = (HexGrid chosenSpot) => {
                target.Move(chosenSpot);
                target.place.setColor(1);
                this.stamina -= THROW_COST;
                scan();

                foreach (GridManager.GridPoint point in reach.grid) {
                    if (point.hex.getState() == "coop")
                        point.hex.changeState("default");
                }
            };
        }
    }

}
