using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridEffect {
    public virtual string name { get; set; } = "none";
    public virtual bool empty { get; set; } = false;
    public HexGrid.ColorSet colorSet;
    HexGrid hexagon;
    public Token user = null;
    SpriteRenderer art;

    public void Setup(HexGrid hexagon, Token user, GameObject representation) {
        this.hexagon = hexagon;
        this.colorSet = colorSet = hexagon.GetColors(name);
        this.user = user;
        representation.name = this.name + "_effect";
        this.art = representation.GetComponent<SpriteRenderer>();
        OnAdded(hexagon);
    }

    public virtual void OnAdded(HexGrid hexagon) {
        colorSet = hexagon.GetColors("default");
        changeColor(hexagon, 0);
    }
    public virtual void OnRemoved(HexGrid hexagon) {
        Object.Destroy(art.gameObject);
    }

    public virtual void OnPointerEnter(HexGrid hexagon) { changeColor(hexagon, 1); }
    public virtual void OnPointerExit(HexGrid hexagon) { changeColor(hexagon, 0); }
    public virtual void OnClick(HexGrid hexagon, int mouseButton) { }

    public virtual void OnTurnStart(HexGrid hexagon) { }
    public virtual void OnTurnEnd(HexGrid hexagon) { }
    public virtual void OnSetToken(HexGrid hexagon, Token token) { }
    public virtual void OnRemoveToken(HexGrid hexagon, Token token) { }

    public void changeColor(HexGrid hexagon, int color) { art.color = colorSet.colors[color]; }
    public void Cancel() { hexagon.removeEffect(this); }
}
