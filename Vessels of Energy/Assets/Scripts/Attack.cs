using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    [HideInInspector] public Character self;
    Token target;
    int roll;
    bool missed = true, counterable = true;
    int extra_evasion = 0;

    private void Awake() {
        self = this.GetComponent<Character>();
    }

    public void PrepareAttack(Token target) {
        Raycast.block = true;
        this.target = target;
        self.stamina -= Character.ATTACK_COST;

        self.animator.Attack();

        if (target is Character) {
            Character enemy = (Character)target;
            enemy.animator.Attacked();

            //rolling dice and giving chance to Evade
            roll = DiceRoller.instance.Roll(self, 8, 2 * self.stats.strength + 4);
            extra_evasion = 0;
            if (enemy.stamina >= Character.EVADE_COST) {
                QTE.instance.startQTE(QTE.Reaction.DODGE, enemy, () => {
                    Debug.Log(enemy.Colored("DODGE!"));
                    enemy.stamina -= Character.EVADE_COST;
                    extra_evasion = enemy.rollDices(2 * enemy.stats.dexterity + 4);
                },
                () => { Debug.Log("Missed Opportunity to Evade..."); });
            }
        }

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

        if (target is Character) {
            Character enemy = (Character)target;
            missed = roll < enemy.stats.evasion + extra_evasion;
        } else {
            missed = false;
        }

        if (!missed) {
            //damage = 1d12 (Weapon) + 1d(2*strength+4)
            //or
            //damage = 1d12 (Weapon) + 1d(2*intelligence+4)
            int damage;
            if (self.weapon.damagetype == 'p') {
                Debug.Log("Physical Attack!");
                damage = DiceRoller.instance.Roll(self, self.weapon.baseDamageDice, 2 * self.stats.strength + 4) - target.stats.defense;
            } else { // damagetype == 'm'
                Debug.Log("Magic Attack!");
                damage = DiceRoller.instance.Roll(self, self.weapon.baseDamageDice, 2 * self.stats.intelligence + 4) - target.stats.resistence;
            }

            if (damage > 0) {
                target.HP -= damage;
                Debug.Log("Attack Hit! Damage " + damage);

                if (target.HP <= 0) {
                    target.HP = 0;
                    target.place.RemoveToken(target);
                    target.Animate("destroy");

                    /*
                    if (self.checkRange(0, self.stamina, target.place))
                        target.place.changeState("reach");
                    else
                        target.place.changeState("default");*/
                    //target.gameObject.SetActive(false);
                } else {
                    target.Animate("damage");
                }
            } else {
                Debug.Log("Attack Blocked");
                target.Animate("block");
            }
        } else {
            Debug.Log("Attack Missed!");
            target.Animate("evade");
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
        if (missed && target is Character) {
            Character enemy = (Character)target;

            if (enemy.stamina >= Character.ATTACK_COST && enemy.checkRange(enemy.weapon.minRange, enemy.weapon.maxRange, self.place)) {
                QTE.instance.startQTE(QTE.Reaction.COUNTER, enemy, () => {
                    Debug.Log("COUNTER!");
                    self.animator.TurnAround();
                    enemy.animator.TurnAround();
                    enemy.Attack(self);
                },
                () => {
                    Debug.Log("Missed Opportunity to Counter...");
                    enemy.animator.RetreatAction();
                    Reset();
                });
            } else {
                enemy.animator.RetreatAction();
                Reset();
            }
        } else {
            GameManager.instance.CheckEndGame();
            Reset();
        }
        self.animator.RetreatAction();
        self.updateReach(false);
    }

    private void Reset() {
        if (Token.selected == this) target.Unselect();
        Raycast.block = false;
        if (CamControl.instance != null) CamControl.instance.Unfocus();
    }
}
