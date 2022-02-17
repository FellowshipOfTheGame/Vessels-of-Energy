using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToken : MonoBehaviour {
    Token token;
    float boardStepSpeed = 0.05f;

    void Awake() {
        token = this.GetComponent<Token>();
    }
    public void Move(GridManager.Grid path, bool walk) {
        Raycast.block = true;
        if (walk) {
            HexGrid nextStep = path.grid[0].hex, destiny = path.grid[path.grid.Count - 1].hex;
            int distance = path.grid.Count;
            boardStepSpeed = token.boardMoveSpeed * (1f / 8f - 0.1f * Mathf.Log10(distance));
            token.OnStartMoving(destiny);
            Debug.Log("Can Move?" + token.canBeMoved);
            if (token.canBeMoved) {
                StartCoroutine(WalkTo(nextStep, boardStepSpeed, () => { SetNewPlace(path, nextStep, 1); }));
            }
        } else {
            SetPosition(path.grid[path.grid.Count - 1].hex);
        }
        Raycast.block = false;
    }

    void SetPosition(HexGrid destiny) {
        HexGrid lastHex = token.place;
        token.place = destiny;
        token.transform.position = destiny.transform.position;

        lastHex.changeState("default");
        lastHex.token = null;

        destiny.token = token;
        destiny.changeState("token");
        destiny.SetToken(token);
        Debug.Log("step");
        token.OnStepOut(lastHex);
    }

    void SetNewPlace(GridManager.Grid path, HexGrid destiny, int step) {
        SetPosition(destiny);
        WalkStep(path, step);
    }

    System.Action WalkStep(GridManager.Grid path, int step) {
        if (path == null) return null;
        if (token.canBeMoved && step < path.grid.Count) {
            HexGrid destiny = path.grid[step].hex;
            StartCoroutine(WalkTo(destiny, boardStepSpeed, () => { SetNewPlace(path, destiny, step + 1); }));
        } else {
            foreach (GridManager.GridPoint point in path.grid) {
                if (point.hex != token.place) point.hex.changeState("default");
            }
            path.origin.changeState("default");
            token.OnStopMoving(path.origin);
        }

        return null;
    }

    IEnumerator WalkTo(HexGrid destiny, float delay, System.Action onStop) {
        //token.place.changeState("reach");
        token.place.setColor(1);
        token.place.token = null;

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
