using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceResultUI : MonoBehaviour {

    public TextMeshProUGUI result;
    public TextMeshProUGUI label;

    public void Initialize(int value, string message, Color color) {
        result.text = value.ToString();
        result.outlineColor = color;
        label.text = message;
        label.outlineColor = color;
    }

    public void Erase() {
        Destroy(this.gameObject);
    }
}
