using System.Collections.Generic;
using UnityEngine;

public class HexGrid : RaycastTarget {
    HexHandler handler;

    public SpriteRenderer art;
    public ColorSet[] pallete;

    [Header("Neighbor Detection")]
    public LayerMask gridLayer, tokenLayer;
    public float radarRadius, radarHeight;

    [Header("Grid Info")]
    public Token token;

    public List<HexGrid> neighbors;

    public override void Awake() {
        base.Awake();
        neighbors = new List<HexGrid>();
        handler = GetComponent<HexHandler>();
        handler.Initialize();

        //detect grid neighbors
        Vector3 point1 = this.transform.position + Vector3.up * radarHeight;
        Vector3 point2 = this.transform.position + Vector3.down * radarHeight;
        Collider[] detected = Physics.OverlapCapsule(point1, point2, radarRadius, gridLayer);
        foreach (Collider c in detected) {
            RaycastCollider collider = c.GetComponent<RaycastCollider>();
            if (collider != null && collider.target is HexGrid && collider.target != this) {
                neighbors.Add((HexGrid)collider.target);
            }
        }

        //detect token on top
        detected = Physics.OverlapSphere(this.transform.position, 0.05f, tokenLayer);
        foreach (Collider c in detected) {
            Token t = c.transform.GetComponent<Token>();
            token = t;
            if (t != null) {
                changeState("token");
                t.place = this;
                t.transform.position = this.transform.position;
                break;
            }
        }
    }
    public bool isEmpty() { return handler.isEmpty(); }
    public void setColor(int value) { handler.setColor(value); }
    public void changeState(string stateName) { handler.changeState(stateName); }
    public string getState() { return handler.state.name; }
    public HexGridEffect addEffect(string effectName) { return handler.addEffect(effectName); }
    public void removeEffect(HexGridEffect effect) { handler.removeEffect(effect); }
    public bool hasEffect(string effectName) {
        foreach (HexGridEffect e in handler.effects) {
            if (e.name == effectName)
                return true;
        }
        return false;
    }

    public override void OnPointerEnter() {
        if (token != null && HUDManager.instance != null) HUDManager.instance.Show(token);
        handler.OnPointerEnter();
    }
    public override void OnPointerExit() {
        if (token != null && HUDManager.instance != null) HUDManager.instance.Hide(token);
        handler.OnPointerExit();
    }
    public override void OnClick(int mouseButton) {
        handler.OnClick(mouseButton);
    }

    public virtual void OnTurnStart() {
        handler.OnTurnStart();
        if (token != null) token.OnTurnStart();
    }
    public virtual void OnTurnEnd() {
        handler.OnTurnEnd();
        if (token != null) token.OnTurnEnd();
    }

    public virtual void RemoveToken(Token token) {
        handler.OnRemoveToken(token);
    }



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
