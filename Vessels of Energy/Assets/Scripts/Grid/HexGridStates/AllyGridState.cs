using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyGridState : HexGridState {
    public override string name { get; set; } = "ally";
    public override bool isEmpty() { return true; }
    public override void OnEnter(HexGrid hexagon) {
        colorSet = hexagon.GetColors("ally");
        changeColor(hexagon, 0);
    }

    public override void OnClick(HexGrid hexagon, int mouseButton) {
        Character self = (Character)hexagon.token;
        Character ally = (Character)Token.selected;

        if (mouseButton == 0) {
            self.Select();
        } else if (mouseButton == 1) {
            ally.Action(self);
        }
    }
}
