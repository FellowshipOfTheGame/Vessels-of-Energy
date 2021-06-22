using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour
{
    public Slider healthSlider;
    public Gradient healthGradient;
    public Image healthFill;
    public Text staminaValue;

    void Update()
    {
        Character c = GetComponent<Character>();
        //TODO: Test Health Bar
        healthSlider.maxValue = c.stats.maxHP;
        healthSlider.value = c.HP;

        //Changes bar color depending on the % of health
        healthFill.color = healthGradient.Evaluate(healthSlider.normalizedValue);

        staminaValue.text = c.stamina.ToString();
    }
}
