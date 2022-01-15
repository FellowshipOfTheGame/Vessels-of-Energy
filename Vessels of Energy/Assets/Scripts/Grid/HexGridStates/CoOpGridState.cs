using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoOpGridState : HexGridState {
    public static Character target = null;
    public override string name { get; set; } = "coop";
    private GridManager.Grid path = null;

    public override void OnEnter(HexGrid hexagon) {
        colorSet = hexagon.GetColors("coop");
        changeColor(hexagon, 0);
    }

    public override void OnClick(HexGrid hexagon, int mouseButton) {
        if (mouseButton == 1) {
            Token.selected.Unselect();
            return;
        } else if (mouseButton == 0) {
            //hexagon.token.OnTarget();
            //Token.selected.Move(hexagon);
            Character c = (Character)Token.selected;
            c.DelayedAction(hexagon);
        }


    }


}
