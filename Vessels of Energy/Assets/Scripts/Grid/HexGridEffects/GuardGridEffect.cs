using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardGridEffect : HexGridEffect {
    public override string name { get; set; } = "guard";
    GridManager.Grid path = null;

    public override void OnAdded(HexGrid hexagon) {
        colorSet = hexagon.GetColors("guard");
        path = null;
        changeColor(hexagon, 0);
    }

    public override void OnClick(HexGrid hexagon, int mouseButton) {
        //hexagon.token.Select();
    }

    public override void OnSetToken(HexGrid hexagon, Token token) {
        if (token is Character) {
            Character target = (Character)token;
            Character shooter = (Character)user;

            if (target.team != shooter.team) {
                target.canBeMoved = false;

                QTE.instance.startQTE(QTE.Reaction.AMBUSH, shooter, () => {
                    Debug.Log(shooter.Colored("AMBUSH!"));
                    shooter.Attack(target);
                    target.canBeMoved = true;
                },
                () => {
                    Debug.Log("Missed Opportunity to Ambush...");
                    target.canBeMoved = true;
                });
            }
        }
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
