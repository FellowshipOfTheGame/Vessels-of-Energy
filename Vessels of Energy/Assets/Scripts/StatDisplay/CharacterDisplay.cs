using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour
{

    Character character = null;

    public GameObject display;

    [Space(5)]
    public Slider healthSlider;
    public Gradient healthGradient;
    public Image healthFill;
    [Space(5)]
    public Text staminaValue;
    /*public Text strengthValue;
    public Text evasionValue;*/
    public Text defenseValue;
    public Text proficiencyDice;
    public Text strengthDice;

    private void Start() {
        display.SetActive(false);
    }

    void Update() {
        if (character != null) {
            healthSlider.maxValue = character.stats.maxHP;
            healthSlider.value = character.HP;

            //Changes bar color depending on the % of health
            healthFill.color = healthGradient.Evaluate(healthSlider.normalizedValue);

            staminaValue.text = character.stamina.ToString();
            /*strengthValue.text = character.strength.ToString();
            evasionValue.text = character.evasion.ToString();*/
            defenseValue.text = character.stats.evasion.ToString() + "/" + character.stats.defense.ToString();
            strengthDice.text = "d" + (character.stats.strength * 2 + 4).ToString();

            //TODO: Get these information from Character and Character's Weapon
            proficiencyDice.text = "d8/d12";
        }
    }

    public void Display(Character character) {
        this.character = character;
        display.SetActive(true);
    }

    public void Hide() {
        this.character = null;
        display.SetActive(false);
    }
}
