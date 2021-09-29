using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTE : MonoBehaviour
{
    public static QTE instance;
    public float countDownTime = 1f;
    KeyCode key;
    [Space(5)]
    public QTEIcon ui;

    private void Awake () {
        if ( instance != null && instance != this ) Destroy(this.gameObject);
        else instance = this;
    }

    // Update is called once per frame
    void Update() {
    }

    //Initiates a QTE
    public void startQTE( string type, Character actor, Action onPress, Action onMiss ) {
        //Set type of QTE
        if(type == "counter"){
            key = KeyCode.C;
            ui.ShowKey('C', "COUNTER!", countDownTime, actor);
        }
        else if (type == "evasion"){
            key = KeyCode.E;
            ui.ShowKey('C', "DODGE!", countDownTime, actor);
        }
        else{
            Debug.Log("Invalid type of QTE");
            return;
        }

        Debug.Log("QTE activated");
        
        StopAllCoroutines();
        StartCoroutine( CountDown(onPress, onMiss) );
    }

    IEnumerator CountDown(Action onPress, Action onMiss){
        float elapsedTime = 0f;
        bool countingDown = true;
        bool correctKey = false;

        while (countingDown) {
            if (Input.GetKeyDown(key)) {
                onPress();
                countingDown = false;
                correctKey = true;
                break;
            }

            if (elapsedTime >= countDownTime) {
                countingDown = false;
                break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ui.Stop(correctKey);
        if ( !correctKey ) onMiss();
    }
}
