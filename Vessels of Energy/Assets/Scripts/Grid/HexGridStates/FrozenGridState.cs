using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenGridState : HexGridState {
    public static Character target = null;
    public override string name { get; set; } = "frozen";
    GridManager.Grid path = null;

    public override void OnEnter(HexGrid hexagon) {
        colorSet = hexagon.GetColors("frozen");
        path = null;
        changeColor(hexagon, 0);

        if (hexagon.token != null) {
            //hexagon.token.canMove = false;
            hexagon.token.OnStartMoving += (HexGrid destiny) => { Debug.Log("FROZEN"); };
        }
    }

    public override void OnClick(HexGrid hexagon, int mouseButton) {
        //hexagon.token.Select();
        Debug.Log("Aqui: frozen");
        if (hexagon.token.isFrozen) {
            Debug.Log("Saiu frozen");
            Character c = (Character)hexagon.token;
            c.stamina -= Character.ATTACK_COST;
            hexagon.token.isFrozen = false;
            hexagon.token.place.changeState("token");
        }
    }

    public override void OnPointerExit(HexGrid hexagon) {
        changeColor(hexagon, 0);

        if (path != null) {
            foreach (GridManager.GridPoint point in path.grid) {
                point.hex.changeState("token");
            }
        }
    }

    public void OnPointerEnter(HexGrid hexagon, Character target) {
        base.OnPointerEnter(Token.selected.place);
    }
}
