using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardedGridState : HexGridState {
    public static Character target = null;
    public static Character user = null;
    public override string name { get; set; } = "guard";
    GridManager.Grid path = null;
    public char team = '-';

    public override void OnEnter(HexGrid hexagon) {
        colorSet = hexagon.GetColors("guard");
        path = null;
        changeColor(hexagon, 0);
        this.team = GameManager.currentTeam;
    }

    public override void OnClick(HexGrid hexagon, int mouseButton) {
        hexagon.token.Select();
    }

    public override void OnPointerExit(HexGrid hexagon) {
        changeColor(hexagon, 0);

        if (path != null) {
            foreach (GridManager.GridPoint point in path.grid) {
                point.hex.changeState("default");
            }
        }
    }

    public void OnPointerEnter(HexGrid hexagon, Character target) {
        base.OnPointerEnter(Token.selected.place);

        GridManager gridM = GridManager.instance;
        path = gridM.getPath(Token.selected.place, target.place);

        Debug.Log(path.grid);
        if (path != null) {
            foreach (GridManager.GridPoint point in path.grid) {
                point.hex.setColor(1);
                if (point.hex.token == target)
                    point.hex.setColor(0);
                //point.hex.changeState("ally");
            }
        }
    }
}
