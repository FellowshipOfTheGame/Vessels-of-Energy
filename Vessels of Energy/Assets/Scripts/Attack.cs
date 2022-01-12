using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    [HideInInspector] public Character self;
    Character target;
    int roll;
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
        //missed = DiceRoller.instance.Roll(self, 8, 2 * self.strength + 4) < target.evasion;
        roll = DiceRoller.instance.Roll(self, 8, 2 * self.stats.strength + 4);

        extra_evasion = 0;

        if(target.stamina >= Character.EVADE_COST){
            QTE.instance.startQTE("evasion", target, () => {
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
        DiceRoller.instance.ShowNumbers("ATAQUE!");

        //Debug.Log("extra_evasion:");
        //Debug.Log(extra_evasion);

        //missed = self.rollDices(8, 2 * self.stats.strength + 4) < target.stats.evasion + extra_evasion;
        //missed = self.rollDices(8, 2 * self.stats.strength + 4) < extra_evasion; //extra_evasion test
        missed = roll < target.stats.evasion + extra_evasion;

        if (!missed) {
            //damage = 1d12 (Weapon) + 1d(2*strength+4)
            //or
            //damage = 1d12 (Weapon) + 1d(2*intelligence+4)
            int damage;
            if(self.weapon.damagetype == 'p'){
                Debug.Log("Physical Attack!");
                damage = DiceRoller.instance.Roll(self, self.weapon.baseDamageDice, 2 * self.stats.strength + 4) - target.stats.defense;
            }
            else{ // damagetype == 'm'
                Debug.Log("Magic Attack!");
                damage = DiceRoller.instance.Roll(self, self.weapon.baseDamageDice, 2 * self.stats.intelligence + 4) - target.stats.resistence;
            }

            if (damage > 0) {
                target.HP -= damage;
                Debug.Log("Attack Hit! Damage " + damage);

                if (target.HP <= 0) {
                    target.animator.Die();
                    target.HP = 0;

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

    public void LandAttack() {
        if (!missed) {
            DiceRoller.instance.ShowNumbers("DANO!");
        }
    }

    public void FinishAttack() {
        DiceRoller.instance.Clear();

        // permitir contra ataque (reação) se o ataque errou
        if (missed) {
            if (target.stamina >= Character.ATTACK_COST && target.checkRange(target.weapon.minRange, target.weapon.maxRange)) {
                QTE.instance.startQTE("counter", target, () => {
                    Debug.Log("COUNTER!");
                    self.animator.TurnAround();
                    target.animator.TurnAround();
                    target.attack.PrepareAttack(self);
                },
                () => {
                    Debug.Log("Missed Opportunity to Counter...");
                    target.animator.RetreatAction();
                    Reset();
                });
            } else {
                target.animator.RetreatAction();
                Reset();
            }
        } else {
            GameManager.instance.checkWinner();
            Reset();
        }
        self.animator.RetreatAction();
    }

    private void Reset() {
        if (Token.selected == this) target.Unselect();
        Raycast.block = false;
        if (CamControl.instance != null) CamControl.instance.Unfocus();
    }
}
