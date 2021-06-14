using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyGridState : HexGridState
{
    public static Character target = null;
    public override string name { get; set; } = "ally";
    GridManager.Grid path = null;

    public override void OnEnter(HexGrid hexagon)
    {
        colorSet = hexagon.GetColors("ally");
        path = null;
        changeColor(hexagon, 0);
    }

    public override void OnClick(HexGrid hexagon, int mouseButton)
    {
        hexagon.token.Select();
    }

    public override void OnPointerExit(HexGrid hexagon)
    {
        changeColor(hexagon, 0);

        if (path != null)
        {
            foreach (GridManager.GridPoint point in path.grid)
            {
                point.hex.changeState("default");
            }
        }
    }

    public void OnPointerEnter(HexGrid hexagon, Character target)
    {
        base.OnPointerEnter(Token.selected.place);

        GridManager gridM = GridManager.instance;
        path = gridM.getPath(Token.selected.place, target.place);

        Debug.Log(path.grid);
        if (path != null)
        {
            foreach (GridManager.GridPoint point in path.grid)
            {
                point.hex.state.changeColor(point.hex, 1);
                if (point.hex.token == target)
                    point.hex.state.changeColor(point.hex, 0);
                //point.hex.changeState("ally");
            }
        }
    }
}
