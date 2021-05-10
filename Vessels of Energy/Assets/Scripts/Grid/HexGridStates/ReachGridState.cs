using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachGridState : HexGridState {
    public override string name { get; set; } = "reach";
    GridManager.Grid path = null;
    
    public override void OnEnter(HexGrid hexagon) {
        colorSet = hexagon.GetColors("reach");
        path = null;
        changeColor(hexagon, 0);
    }

    public override void OnPointerEnter(HexGrid hexagon) {
        base.OnPointerEnter(hexagon);

        GridManager gridM = GridManager.instance;
        path = gridM.getPath(Token.selected.place, hexagon, "token","enemy");

        if (path != null) {
            foreach(GridManager.GridPoint point in path.grid) {
                point.hex.state.changeColor(point.hex, 1);
            }
        }
    }

    public override void OnPointerExit(HexGrid hexagon) {
        changeColor(hexagon, 0);

        if (path != null) {
            foreach (GridManager.GridPoint point in path.grid) {
                point.hex.state.changeColor(point.hex, 0);
            }
        }
    }

    public override void OnClick(HexGrid hexagon, int mouseButton) {
        if (mouseButton == 1) {
            Token.selected.Unselect();
            return;
        }

        if (path == null) return;

        Token.selected.Move(path);
    }
}