using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrozenGridEffect : HexGridEffect {
    public override string name { get; set; } = "frozen";
    public Prop block;
    //TO DO: Cover system
    public override void OnAdded(HexGrid hexagon) {
        colorSet = hexagon.GetColors("frozen");
        changeColor(hexagon, 0);

        if (hexagon.token != null) {
            hexagon.token.canBeMoved = false;
            hexagon.token.OnStartMoving += (HexGrid destiny) => { Debug.Log("FROZEN"); };
        }
    }

    public override void OnClick(HexGrid hexagon, int mouseButton) {
        if (mouseButton == 2 && Token.selected != null && Token.selected is Character) {
            Character c = (Character)Token.selected;
            c.Attack(block);
        }
    }

    public override void OnRemoved(HexGrid hexagon) {
        base.OnRemoved(hexagon);

        if (hexagon.token != null) {
            hexagon.token.canBeMoved = true;
            hexagon.token.OnStartMoving -= (HexGrid destiny) => { Debug.Log("FROZEN"); };
        }
    }

    public override void OnRemoveToken(HexGrid hexagon, Token token) {
        if (token == block) {
            Cancel();
            if (hexagon.token == block) hexagon.token = null;
            return;
        }

        if (hexagon.token == token) {
            hexagon.token = block;
        }
    }

    public void SpawnIce(HexGrid hexagon, GameObject prefab) {
        block = GameObject.Instantiate(prefab, hexagon.transform).GetComponent<Prop>();
        block.transform.position = hexagon.transform.position;
        block.place = hexagon;
    }
}
