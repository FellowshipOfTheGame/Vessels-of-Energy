using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : RaycastTarget {

    public HexGridState state;

    List<HexGridState> stateList;

    public SpriteRenderer art;
    public ColorSet[] pallete;

    [Header("Neighbor Detection")]
    public LayerMask gridLayer, tokenLayer;
    public float radarRadius, radarHeight;

    [Header("Grid Info")]
    public Token token;
    public List<HexGrid> neighbors;

    public override void Awake(){
        base.Awake();
        neighbors = new List<HexGrid>();

        //initialize states
        stateList = new List<HexGridState>();
        stateList.Add( new HexGridState()   );
        stateList.Add( new TokenGridState() );
        stateList.Add( new ReachGridState() );
        stateList.Add( new EnemyGridState() );

        state = State("default");

        //detect grid neighbors
        Vector3 point1 = this.transform.position + Vector3.up * radarHeight;
        Vector3 point2 = this.transform.position + Vector3.down * radarHeight;
        Collider[] detected = Physics.OverlapCapsule(point1, point2, radarRadius, gridLayer);
        foreach(Collider c in detected) {
            RaycastCollider collider = c.GetComponent<RaycastCollider>();
            if (collider != null && collider.target is HexGrid && collider.target != this){
                neighbors.Add((HexGrid)collider.target);
            }
        }

        //detect token on top
        detected = Physics.OverlapSphere(this.transform.position, 0.05f, tokenLayer);
        foreach (Collider c in detected) {
            Token t = c.transform.GetComponent<Token>();
            token = t;
            if (t != null) {
                state = State("token");
                t.place = this;
                t.transform.position = this.transform.position;
                break;
            }
        }

        state.OnEnter(this);
    }

    public void changeState(string stateName) {
        HexGridState newState = State(stateName);
        if (newState == null) return;

        state.OnExit(this);
        state = newState;
        state.OnEnter(this);
    }

    HexGridState State(string name) {
        foreach(HexGridState s in stateList){
            if (s.name == name)
                return s;
        }
        Debug.Log(name + " state not found...");
        return null;
    }

    public override void OnPointerEnter() { state.OnPointerEnter(this); }
    public override void OnPointerExit() { state.OnPointerExit(this); }
    public override void OnClick(int mouseButton){ state.OnClick(this, mouseButton); }

    public virtual void OnTurnStart() { state.OnTurnStart(this); }
    public virtual void OnTurnEnd() { state.OnTurnEnd(this); }

    [System.Serializable]
    public class ColorSet {
        public string label;
        public Color[] colors;
        public ColorSet(string label, params Color[] colors) {
            this.label = label;
            this.colors = colors;
        }
    }

    public ColorSet GetColors(string name) {
        foreach (ColorSet colors in pallete) {
            if (colors.label == name)
                return colors;
        }
        Debug.Log("color set not found");
        return new ColorSet("none", Color.gray, Color.white);
    }
}
