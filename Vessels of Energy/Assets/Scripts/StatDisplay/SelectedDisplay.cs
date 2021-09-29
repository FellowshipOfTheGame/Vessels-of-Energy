using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedDisplay : MonoBehaviour
{
    public Slider healthSlider;
    public Gradient healthGradient;
    public Image healthFill;

    public Text staminaValue;
    /*public Text strengthValue;
    public Text evasionValue;*/
    public Text defenseValue;
    public Text proficiencyDice;
    public Text damageDice;

    void Update()
    {
        if (Token.selected != null){
            Character c = (Character)Token.selected;

            healthSlider.maxValue = c.stats.maxHP;
            healthSlider.value = c.HP;

            //Changes bar color depending on the % of health
            healthFill.color = healthGradient.Evaluate(healthSlider.normalizedValue);

            staminaValue.text = c.stamina.ToString();
            /*strengthValue.text = c.strength.ToString();
            evasionValue.text = c.evasion.ToString();*/
            defenseValue.text = c.stats.defense.ToString();

            //TODO: Get information from Character
            proficiencyDice.text = "1d8";
            damageDice.text = "1d" + c.weapon.baseDamageDice.ToString();
        }
        else{
            healthSlider.maxValue = 1;
            healthSlider.value = 0;

            staminaValue.text = "";
            /*strengthValue.text = "";
            evasionValue.text = "";*/
            defenseValue.text = "";
            proficiencyDice.text = "";
            damageDice.text = "";
        }
    }
}
