using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGridState : HexGridState {
    public static Character target = null;
    public override string name { get; set; } = "enemy";

    public override bool isEmpty() { return true; }
    public override void OnEnter(HexGrid hexagon) {
        colorSet = hexagon.GetColors("enemy");
        changeColor(hexagon, 0);
    }

    public override void OnClick(HexGrid hexagon, int mouseButton) {
        Character self = (Character)hexagon.token;
        Character enemy = (Character)Token.selected;

        if (mouseButton == 0) {
            enemy.attack.PrepareAttack(self);
        } else if (mouseButton == 1) {
            enemy.Action(self);
        }
    }
}
