using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour {
    public bool isFrozen = false;
    public static Token selected = null;
    public static bool locked = false;
    [HideInInspector] public HexGrid place;

    public void Select() {
        if (!locked) {
            if (selected != null) {
                selected.Unselect();
                if (HUDManager.instance != null) HUDManager.instance.Show(this);
            }
            selected = this;
            this.OnSelect();
        } else {
            this.TargetSelect();
            selected.Action();
            selected.OnFinishAction();
        }
    }

    public void OnTarget() {
        this.TargetSelect();
        selected.Action();
        selected.OnFinishAction();
    }

    public virtual void OnSelect() { }
    public virtual void TargetSelect() { }
    public virtual void Action() { }
    public virtual void OnFinishAction() { }

    public void Unselect() {
        if (selected == this) {
            selected = null;
            if (HUDManager.instance != null) HUDManager.instance.Hide(this);
        }
        this.OnCancelSelect();
    }
    public virtual void OnCancelSelect() { }

    public virtual void OnHighlight() { }
    public virtual void OnCancelHighlight() { }
    public virtual void OnTurnStart() { }
    public virtual void OnTurnEnd() { }

    public void Move(GridManager.Grid path) {
        place.changeState("default");
        place.token = null;

        HexGrid destiny = path.grid[path.grid.Count - 1].hex;
        Raycast.block = true;
        this.OnMove(path, destiny);

        place = destiny;
        destiny.token = this;
        destiny.changeState("token");
        Raycast.block = false;
        this.OnFinishMove();
    }

    public void Move(HexGrid destiny) {
        GridManager.Grid path = new GridManager.Grid(place);
        Move(path);
    }

    public virtual void OnMove(GridManager.Grid path, HexGrid destiny) {
        this.transform.position = destiny.transform.position;
        //Debug.Log("MOVED");
    }

    public virtual void OnFinishMove() { }
}
