using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour {
    [Header("Token")]
    public bool isFrozen = false;
    [HideInInspector] public bool canBeMoved = true;
    public static Token selected = null, targeted = null;
    public static bool locked = false;
    [HideInInspector] public HexGrid place;
    public float boardMoveSpeed = 1f;
    public Stats stats;
    public int HP;

    public delegate void HexEvent(HexGrid hex);
    public HexEvent OnStepOut, OnStopMoving, OnStartMoving;

    MoveToken movement;

    protected void Awake() {
        movement = this.GetComponent<MoveToken>();
        this.stats.calculateStats();
        OnStepOut = (HexGrid hex) => { hex.SetToken(this); };
        OnStopMoving = (HexGrid hex) => { };
        OnStartMoving = (HexGrid hex) => { };
    }

    public void Select() {
        if (selected != null) {
            selected.Unselect();
            HUDManager.instance.Show(this);
        }
        selected = this;
        this.OnSelect();
    }

    public void Unselect() {
        if (selected == this) {
            selected = null;
            if (HUDManager.instance != null) HUDManager.instance.Hide(this);
        }
        this.OnCancelSelect();
    }

    public void Move(GridManager.Grid path) { movement.Move(path, true); }
    public void Move(HexGrid destiny) { //teleport
        GridManager.Grid path = new GridManager.Grid(place);
        path.grid.Add(new GridManager.GridPoint(destiny, 1));
        movement.Move(path, false);
    }

    public virtual void Animate(string command) { }

    public virtual void OnTarget() { }
    public virtual void OnSelect() { }
    public virtual void OnCancelSelect() { }

    public virtual void OnHighlight() { }
    public virtual void OnCancelHighlight() { }
    public virtual void OnTurnStart() { }
    public virtual void OnTurnEnd() { }
}
