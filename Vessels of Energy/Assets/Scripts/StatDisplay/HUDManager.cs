using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour {
    public static HUDManager instance;

    public CharacterDisplay selectedDisplay, targetDisplay;

    private void Awake() {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Show(Token token) {
        Character c = (Character)token;

        if (Token.selected == null) {
            selectedDisplay.Display(c);
        } else {
            Character s = (Character)Token.selected;

            if (s.team == c.team) {
                selectedDisplay.Display(c);
            } else {
                targetDisplay.Display(c);
            }
        }

    }

    public void Hide(Token token) {
        Character c = (Character)token;

        if (Token.selected == null) {
            selectedDisplay.Hide();
        } else if (Token.selected != token) {
            Character s = (Character)Token.selected;

            if (s.team == c.team) {
                selectedDisplay.Display(s);
            } else {
                targetDisplay.Hide();
            }
        }
    }

    public void Clear() {
        selectedDisplay.Hide();
        targetDisplay.Hide();
    }
}
