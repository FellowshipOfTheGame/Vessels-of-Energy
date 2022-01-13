using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardGridEffect : HexGridEffect {
    public override string name { get; set; } = "guard";
    GridManager.Grid path = null;
    public Team team = null;

    public override void OnAdded(HexGrid hexagon) {
        colorSet = hexagon.GetColors("guard");
        path = null;
        changeColor(hexagon, 0);

        Character c = (Character)user;
        this.team = c.team;
    }

    public override void OnClick(HexGrid hexagon, int mouseButton) {
        //hexagon.token.Select();
    }

    public override void OnPointerEnter(HexGrid hexagon) {
        changeColor(hexagon, 1);
        user.place.setColor(0);
    }

    public override void OnPointerExit(HexGrid hexagon) {
        changeColor(hexagon, 0);
        user.place.setColor(0);
    }
}
