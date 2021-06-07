using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QTEIcon : MonoBehaviour {

    public TextMeshProUGUI keyName, label;
    public Image keyboard, fill;
    [Space(5)]
    public Gradient fillGradient;

    Animator animator;
    bool waiting = false;
    float time = 0f, timeLimit;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update() {
        if (waiting) {
            float percent = Mathf.Clamp((timeLimit - time) / timeLimit, 0f, 1f);
            fill.fillAmount = percent;
            fill.color = fillGradient.Evaluate(percent);

            time += Time.deltaTime;
        }
    }

    public void ShowKey(char key, string action, float duration, Character actor) {
        keyName.text = key.ToString();
        label.text = action;
        keyboard.color = actor.color;
        waiting = true;

        time = 0f;
        timeLimit = duration;
        animator.SetBool("show", true);
    }

    public void Stop(bool pressed) {
        waiting = false;
        animator.SetBool("show", false);
        animator.SetBool("pressed", pressed);
    }
}
