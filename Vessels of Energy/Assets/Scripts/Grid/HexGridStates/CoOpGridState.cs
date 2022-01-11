using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoOpGridState : HexGridState
{
    public static Character target = null;
    public override string name { get; set; } = "coop";
    private GridManager.Grid path = null;

    public override void OnEnter(HexGrid hexagon)
    {
        colorSet = hexagon.GetColors("coop");
        path = null;
        changeColor(hexagon, 0);
    }


    public override void OnPointerEnter(HexGrid hexagon)
    {
        base.OnPointerEnter(hexagon);
        //Token.selected.place
        GridManager gridM = GridManager.instance;
        path = gridM.getPath(Token.selected.place, hexagon, "token", "enemy", "ally");
        Debug.Log(path);
        Debug.Log(hexagon);
        
        if (path != null)
        {
            foreach (GridManager.GridPoint point in path.grid)
            {
                point.hex.state.changeColor(point.hex, 1);
            }
        }
    }

    public override void OnPointerExit(HexGrid hexagon)
    {
        changeColor(hexagon, 0);

        if (path != null)
        {
            foreach (GridManager.GridPoint point in path.grid)
            {
                point.hex.state.changeColor(point.hex, 0);
            }
        }
    }

    public override void OnClick(HexGrid hexagon, int mouseButton)
    {
        if (mouseButton == 1)
        {
            Token.selected.Unselect();
            return;
        }
        if (path == null) return;

        else if (mouseButton == 0)
        {
            //hexagon.token.OnTarget();
            Token.selected.Move(hexagon);
        }


    }


}
