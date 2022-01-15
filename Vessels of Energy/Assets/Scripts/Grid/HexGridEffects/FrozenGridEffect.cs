using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenGridEffect : HexGridEffect {
    public override string name { get; set; } = "frozen";
    //TO DO: Cover system
    public override void OnAdded(HexGrid hexagon) {
        colorSet = hexagon.GetColors("frozen");
        changeColor(hexagon, 0);

        if (hexagon.token != null) {
            hexagon.token.canBeMoved = false;
            hexagon.token.OnStartMoving += (HexGrid destiny) => { Debug.Log("FROZEN"); };
        }
    }

    public override void OnRemoved(HexGrid hexagon) {
        base.OnRemoved(hexagon);

        if (hexagon.token != null) {
            hexagon.token.canBeMoved = true;
            hexagon.token.OnStartMoving -= (HexGrid destiny) => { Debug.Log("FROZEN"); };
        }
    }
}
