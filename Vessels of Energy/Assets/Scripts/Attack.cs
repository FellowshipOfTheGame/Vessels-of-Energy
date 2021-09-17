﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    [HideInInspector] public Character self;
    Character target;

    bool missed = true, counterable = true;
    int extra_evasion = 0;

    private void Awake() {
        self = this.GetComponent<Character>();
        self.attack = this;
    }

    public void PrepareAttack(Character target) {
        Raycast.block = true;
        this.target = target;
        self.stamina -= Character.ATTACK_COST;

        self.animator.Attack();
        target.animator.Attacked();
        Debug.Log("ATTACK!");
        // logica de chance te ataque

        // precision = 1d8 + 1d(2*stat+4)
        // evasion = evasion //+ 1d(2*dexterity+4)

        extra_evasion = 0;

        if(target.stamina >= Character.EVADE_COST){
            QTE.instance.startQTE("evasion", () => {
                Debug.Log("EVASION!");
                target.stamina -= Character.EVADE_COST;
                extra_evasion = target.rollDices(2 * target.stats.dexterity + 4);
                //extra_evasion = 100; // extra_evasion test
            },
            () => {
                Debug.Log("Missed Opportunity to Evade...");
                //extra_evasion = 0;
            });
        }

        //missed = true; //the ultimate Counter test
        //make camera focus on combat
        if (CamControl.instance != null) {
            Vector3 center = (self.transform.position + target.transform.position) / 2;
            Vector3 direction = Quaternion.Euler(0f, 90f, 0f) * (self.transform.position - target.transform.position).normalized;
            CamControl.instance.Focus(center, direction);
        }

    }

    public void ExecuteAttack() {
        self.animator.ExecuteAction();

        //Debug.Log("extra_evasion:");
        //Debug.Log(extra_evasion);

        missed = self.rollDices(8, 2 * self.stats.strength + 4) < target.stats.evasion + extra_evasion;
        //missed = self.rollDices(8, 2 * self.stats.strength + 4) < extra_evasion; //extra_evasion test

        if (!missed) {
            //damage = 1d12 (Weapon) + 1d(2*strength+4)
            int damage = self.rollDices(self.weapon.baseDamageDice, 2 * self.stats.strength + 4) - target.stats.defense;

            if (damage > 0) {
                target.HP -= damage;
                Debug.Log("Attack Hit! Damage " + damage);

                if (target.HP <= 0) {
                    target.animator.Die();
                    target.HP = 0;
                    GameManager.instance.checkWinner();

                    if (self.checkRange(0, self.stamina))
                        target.place.changeState("reach");
                    else
                        target.place.changeState("default");
                    //target.gameObject.SetActive(false);
                } else {
                    target.animator.Damage();
                }
            } else {
                Debug.Log("Attack Blocked");
                target.animator.Block();
            }
        } else {
            Debug.Log("Attack Missed!");
            target.animator.Evade();
        }
    }

    public void FinishAttack() {
        // permitir contra ataque (reação) se o ataque errou
        // TODO: Check if attack range allows counter
        if (missed && counterable && target.stamina >= Character.ATTACK_COST ) {

            QTE.instance.startQTE("counter", () => {
                Debug.Log("COUNTER!");
                target.attack.PrepareAttack(self);
            },
            () => {
                Debug.Log("Missed Opportunity to Counter...");
                target.animator.RetreatAction();
                Raycast.block = false;
                if ( CamControl.instance != null ) CamControl.instance.Unfocus();
            });
        } else {
            Raycast.block = false;
            if ( CamControl.instance != null ) CamControl.instance.Unfocus();
        }

        self.updateReach();
        counterable = true;
        self.animator.RetreatAction();
    }
}
