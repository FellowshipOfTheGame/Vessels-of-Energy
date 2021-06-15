using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    [HideInInspector] public Character self;
    Character target;

    bool missed = true;

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
        missed = DiceRoller.instance.Roll(self, 8, 2 * self.strength + 4) < target.evasion;

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

        if (!missed) {
            //damage = 1d12 (Weapon) + 1d(2*strength+4)
            int damage = DiceRoller.instance.Roll(self, 12, 2 * self.strength + 4) - target.defense;

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
        // TODO: Check if attack range allows counter
        if (missed) {
            if (target.stamina >= Character.ATTACK_COST) {
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
