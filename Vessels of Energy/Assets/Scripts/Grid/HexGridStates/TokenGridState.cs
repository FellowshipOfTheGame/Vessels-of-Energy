using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenGridState : HexGridState {
    public override string name { get; set; } = "token";
    char team = '-';

    public override void OnEnter(HexGrid hexagon) {
        colorSet = hexagon.GetColors("token");
        if (hexagon.token is Character) {
            Character c = (Character)hexagon.token;
            team = c.team;
        }
        
        if (GameManager.currentTeam != team)
            changeColor(hexagon, 0);
        else
            changeColor(hexagon, 2);
    }

    public override void OnPointerEnter(HexGrid hexagon) {
        if ( GameManager.currentTeam != team )
            changeColor(hexagon, 1);
        else
            changeColor(hexagon, 3);
        hexagon.token.OnHighlight();
    }

    public override void OnPointerExit(HexGrid hexagon) {
        if ( GameManager.currentTeam != team )
            changeColor(hexagon, 0);
        else
            changeColor(hexagon, 2);
        hexagon.token.OnCancelHighlight();
    }

    public override void OnClick(HexGrid hexagon, int mouseButton) {
        hexagon.token.Select();
    }

    public override void OnTurnStart ( HexGrid hexagon ) {
        if ( GameManager.currentTeam != team )
            changeColor(hexagon, 0);
        else
            changeColor(hexagon, 2);
    }
}
