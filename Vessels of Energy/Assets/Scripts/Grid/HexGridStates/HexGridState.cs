using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridState {
    public virtual string name { get; set; } = "default";
    protected HexGrid.ColorSet colorSet;

    public virtual void OnEnter(HexGrid hexagon) {
        colorSet = hexagon.GetColors("default");
        changeColor(hexagon, 0);
    }
    public virtual void OnExit(HexGrid hexagon) {}

    public virtual void OnPointerEnter(HexGrid hexagon) { changeColor(hexagon, 1); }
    public virtual void OnPointerExit(HexGrid hexagon) { changeColor(hexagon, 0); }
    public virtual void OnClick(HexGrid hexagon, int mouseButton) { 
        if (Token.selected != null) {
            Debug.Log("Cancel");
            Token.selected.Unselect();
        }
    }

    public virtual void OnTurnStart(HexGrid hexagon) {}
    public virtual void OnTurnEnd (HexGrid hexagon) {}

    public void changeColor(HexGrid hexagon, int color){
        hexagon.art.color = colorSet.colors[color];
    }
}