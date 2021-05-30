﻿using System.Collections;
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
    public Text damageDice;

    private void Start() {
        display.SetActive(false);
    }

    void Update() {
        if (character != null) {
            healthSlider.maxValue = character.maxHP;
            healthSlider.value = character.HP;

            //Changes bar color depending on the % of health
            healthFill.color = healthGradient.Evaluate(healthSlider.normalizedValue);

            staminaValue.text = character.stamina.ToString();
            /*strengthValue.text = character.strength.ToString();
            evasionValue.text = character.evasion.ToString();*/
            defenseValue.text = character.defense.ToString();

            //TODO: Get these information from Character and Character's Weapon
            proficiencyDice.text = "1d8";
            damageDice.text = "1d12";
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
