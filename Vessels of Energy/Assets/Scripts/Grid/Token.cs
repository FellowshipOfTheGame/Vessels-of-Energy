using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour {
    public bool isFrozen = false;
    public static Token selected = null;
    public static bool locked = false;
    [HideInInspector] public HexGrid place;
    public float boardMoveSpeed = 1f;
    float boardStepSpeed = 0.05f;

    public delegate void MovingEvent(HexGrid hex);
    public MovingEvent OnStepOut, OnStopMoving;

    protected void Awake() {
        OnStepOut = (HexGrid hex) => { };
        OnStopMoving = (HexGrid hex) => { };
    }

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

    public void Move(GridManager.Grid path) { Move(path, true); }
    public void Move(HexGrid destiny) { //teleport
        GridManager.Grid path = new GridManager.Grid(place);
        Move(path, false);
    }
    void Move(GridManager.Grid path, bool walk) {
        Raycast.block = true;
        if (walk) {
            HexGrid nextStep = path.grid[0].hex;
            int distance = path.grid.Count;
            boardStepSpeed = boardMoveSpeed * (1f / 8f - 0.1f * Mathf.Log10(distance));
            StartCoroutine(WalkTo(nextStep, boardStepSpeed, () => { SetNewPlace(path, nextStep, 1); }));
        }

        //OnStopMoving(path.origin);
        Raycast.block = false;
    }

    void SetNewPlace(GridManager.Grid path, HexGrid destiny, int step) {
        HexGrid lastHex = path.grid[step - 1].hex;
        place = destiny;
        destiny.token = this;
        destiny.changeState("token");
        OnStepOut(lastHex);
        WalkStep(path, step);
    }

    System.Action WalkStep(GridManager.Grid path, int step) {
        if (path == null) return null;
        if (step < path.grid.Count) {
            HexGrid destiny = path.grid[step].hex;
            StartCoroutine(WalkTo(destiny, boardStepSpeed, () => { SetNewPlace(path, destiny, step + 1); }));
        } else {
            path.origin.changeState("default");
            foreach (GridManager.GridPoint point in path.grid) {
                if (point.hex != place) point.hex.changeState("default");
                OnStopMoving(path.origin);
            }
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
