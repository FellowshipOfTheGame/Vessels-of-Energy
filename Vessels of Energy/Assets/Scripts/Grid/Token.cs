using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour {
    public bool isFrozen = false;
    [HideInInspector] public bool canBeMoved = true;
    public static Token selected = null, targeted = null;
    public static bool locked = false;
    [HideInInspector] public HexGrid place;
    public float boardMoveSpeed = 1f;
    float boardStepSpeed = 0.05f;

    public delegate void HexEvent(HexGrid hex);
    public HexEvent OnStepOut, OnStopMoving, OnStartMoving;

    protected void Awake() {
        OnStepOut = (HexGrid hex) => { };
        OnStopMoving = (HexGrid hex) => { };
        OnStartMoving = (HexGrid hex) => { };
    }

    public void Select() {/*
        if (!locked) {
            if (selected != null) {
                selected.Unselect();
                if (HUDManager.instance != null) HUDManager.instance.Show(this);
            }
            selected = this;
            this.OnSelect();
        } else {
            this.TargetSelect();
        }*/
        if (selected != null) {
            selected.Unselect();
            HUDManager.instance.Show(this);
        }
        selected = this;
        this.OnSelect();
    }

    public void TargetSelect(int actionIndex) {
        targeted = this;
        this.OnTarget();
        selected.Action(this);
        selected.OnFinishAction();
    }

    public virtual void OnTarget() { }
    public virtual void OnSelect() { }

    public virtual void Action(Token target) { }
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

    public void Move(GridManager.Grid path) { Move(path, true); }
    public void Move(HexGrid destiny) { //teleport
        GridManager.Grid path = new GridManager.Grid(place);
        path.grid.Add(new GridManager.GridPoint(destiny, 1));
        Move(path, false);
    }
    void Move(GridManager.Grid path, bool walk) {
        Raycast.block = true;
        if (walk) {
            HexGrid nextStep = path.grid[0].hex, destiny = path.grid[path.grid.Count - 1].hex;
            int distance = path.grid.Count;
            boardStepSpeed = boardMoveSpeed * (1f / 8f - 0.1f * Mathf.Log10(distance));
            OnStartMoving(destiny);
            Debug.Log("Can Move?" + canBeMoved);
            if (canBeMoved) {
                StartCoroutine(WalkTo(nextStep, boardStepSpeed, () => { SetNewPlace(path, nextStep, 1); }));
            }
        } else {
            SetPosition(path.grid[path.grid.Count - 1].hex);
        }
        Raycast.block = false;
    }

    void SetPosition(HexGrid destiny) {
        HexGrid lastHex = place;
        place = destiny;
        this.transform.position = destiny.transform.position;

        lastHex.changeState("default");
        lastHex.token = null;

        destiny.token = this;
        destiny.changeState("token");
        OnStepOut(lastHex);
    }

    void SetNewPlace(GridManager.Grid path, HexGrid destiny, int step) {
        SetPosition(destiny);
        WalkStep(path, step);
    }

    System.Action WalkStep(GridManager.Grid path, int step) {
        if (path == null) return null;
        if (canBeMoved && step < path.grid.Count) {
            HexGrid destiny = path.grid[step].hex;
            StartCoroutine(WalkTo(destiny, boardStepSpeed, () => { SetNewPlace(path, destiny, step + 1); }));
        } else {
            foreach (GridManager.GridPoint point in path.grid) {
                if (point.hex != place) point.hex.changeState("default");
            }
            path.origin.changeState("default");
            OnStopMoving(path.origin);
        }

        return null;
    }

    IEnumerator WalkTo(HexGrid destiny, float delay, System.Action onStop) {
        //place.changeState("reach");
        place.setColor(1);
        place.token = null;

        Vector3 start = this.transform.position;
        Vector3 end = destiny.transform.position;
        float timeElapsed = 0;
        float distance = (end - start).magnitude;

        while (timeElapsed < delay) {
            float step = timeElapsed / delay;
            float smooth = step * step * (3f - 2f * step); //smooth curve (x = 3t^2 - 2t^3)
            float linear = step; //linear curve (x = t)

            this.transform.position = Vector3.Lerp(start, end, linear);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        this.transform.position = end;
        onStop.Invoke();
    }
}
