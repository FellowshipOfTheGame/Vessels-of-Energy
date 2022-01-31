using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexHandler : MonoBehaviour {

    public HexGridState state;
    public List<HexGridEffect> effects;

    HexGrid hex;
    public delegate void HexEvent();

    List<HexGridState> stateList;
    List<HexGridEffect> effectList;

    bool processing = false;
    List<HexGridEffect> effectsToRemove;

    public void Initialize() {
        //initialize states
        stateList = new List<HexGridState>();
        stateList.Add(new HexGridState());
        stateList.Add(new TokenGridState());
        stateList.Add(new ReachGridState());
        stateList.Add(new EnemyGridState());
        stateList.Add(new AllyGridState());
        stateList.Add(new CoOpGridState());
        //stateList.Add(new GuardedGridState());
        //stateList.Add(new FrozenGridState());

        //initialize effects
        effectList = new List<HexGridEffect>();
        effectList.Add(new GuardGridEffect());
        effectList.Add(new FrozenGridEffect());

        //set control values
        processing = false;
        effectsToRemove = new List<HexGridEffect>();

        //set default values
        hex = this.GetComponent<HexGrid>();
        effects = new List<HexGridEffect>();
        changeState("default");
    }

    public void setColor(int value) { state.changeColor(hex, value); }

    public bool isEmpty() { return state.isEmpty(); }

    public void changeState(string stateName) {
        HexGridState newState = State(stateName);
        if (newState == null) return;

        if (state != null) state.OnExit(hex);
        state = newState;
        state.OnEnter(hex);
    }

    public HexGridEffect addEffect(string effectName) {
        HexGridEffect newEffect = Effect(effectName);
        if (newEffect == null) return null;

        //registering effect
        effects.Add(newEffect);

        //initializing effect
        GameObject newEffectArt = Instantiate(hex.art.transform.GetChild(0).gameObject, hex.art.transform);
        newEffectArt.SetActive(true);
        newEffectArt.transform.position = hex.art.transform.GetChild(0).position;
        newEffect.Setup(hex, null, newEffectArt);

        Debug.Log(effectName + " effect added...");
        return newEffect;
    }

    public void removeEffect(HexGridEffect effect) {
        if (!effects.Contains(effect)) return;
        StartCoroutine(removeEffectWhenSafe(effect));
    }

    IEnumerator removeEffectWhenSafe(HexGridEffect effect) {
        while (processing) yield return null;

        //unregistering effect
        effects.Remove(effect);
        effect.OnRemoved(hex);
    }


    public HexGridState State(string name) {
        foreach (HexGridState s in stateList) {
            if (s.name == name)
                return s;
        }
        Debug.Log(name + " state not found...");
        return null;
    }

    public HexGridEffect Effect(string name) {
        foreach (HexGridEffect e in effectList) {
            if (e.name == name) {
                System.Type t = e.GetType();
                object copy = System.Activator.CreateInstance(t);
                return (HexGridEffect)copy;
            }

        }
        Debug.Log(name + " effect not found...");
        return null;
    }

    public void OnPointerEnter() {
        processing = true;
        foreach (HexGridEffect e in effects) e.OnPointerEnter(hex);
        state.OnPointerEnter(hex);
        processing = false;
    }

    public void OnPointerExit() {
        processing = true;
        foreach (HexGridEffect e in effects) e.OnPointerExit(hex);
        state.OnPointerExit(hex);
        processing = false;
    }

    public void OnClick(int mouseButton) {
        processing = true;
        foreach (HexGridEffect e in effects) e.OnClick(hex, mouseButton);
        state.OnClick(hex, mouseButton);
        processing = false;
    }

    public void OnTurnStart() {
        processing = true;
        foreach (HexGridEffect e in effects) e.OnTurnStart(hex);
        state.OnTurnStart(hex);
        processing = false;
    }

    public void OnTurnEnd() {
        processing = true;
        foreach (HexGridEffect e in effects) e.OnTurnEnd(hex);
        state.OnTurnEnd(hex);
        processing = false;
    }

    public void OnRemoveToken(Token token) {
        processing = true;
        foreach (HexGridEffect e in effects) e.OnRemoveToken(hex, token);
        state.OnRemoveToken(hex, token);
        processing = false;
    }
}
