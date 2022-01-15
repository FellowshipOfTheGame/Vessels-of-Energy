using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexHandler : MonoBehaviour {

    public HexGridState state;
    public List<HexGridEffect> effects;

    HexGrid hex;
    public delegate void HexEvent();
    public HexEvent PointerEnter, PointerExit, LeftClick, RightClick, TurnStart, TurnEnd;


    List<HexGridState> stateList;
    List<HexGridEffect> effectList;

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

        //set events
        hex = this.GetComponent<HexGrid>();
        PointerEnter = () => { state.OnPointerEnter(hex); };
        PointerExit = () => { state.OnPointerExit(hex); };
        LeftClick = () => { state.OnClick(hex, 0); };
        RightClick = () => { state.OnClick(hex, 1); };
        TurnStart = () => { state.OnTurnStart(hex); };
        TurnEnd = () => { state.OnTurnEnd(hex); };

        //set default values
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
        PointerEnter += () => { newEffect.OnPointerEnter(hex); };
        PointerExit += () => { newEffect.OnPointerExit(hex); };
        LeftClick += () => { newEffect.OnClick(hex, 0); };
        RightClick += () => { newEffect.OnClick(hex, 1); };
        TurnStart += () => { newEffect.OnTurnStart(hex); };
        TurnEnd += () => { newEffect.OnTurnEnd(hex); };

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

        //unregistering effect
        effects.Remove(effect);
        PointerEnter -= () => { effect.OnPointerEnter(hex); };
        PointerExit -= () => { effect.OnPointerExit(hex); };
        LeftClick -= () => { effect.OnClick(hex, 0); };
        RightClick -= () => { effect.OnClick(hex, 1); };
        TurnStart -= () => { effect.OnTurnStart(hex); };
        TurnEnd -= () => { effect.OnTurnEnd(hex); };

        //running effect exit
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
}
