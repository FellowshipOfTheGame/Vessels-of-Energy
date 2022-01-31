using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcerer : Character {
    [Header("Sorcerer")]
    private int minRange = 2, maxRange = 4;
    public GameObject bullet;
    //public ParticleSystem attackParticle;
    public const int FREEZE_COST = 2;

    List<FrozenGridEffect> frozenBlocks;

    void Start() {
        this.stats.calculateStats();
        this.HP = stats.maxHP;
        frozenBlocks = new List<FrozenGridEffect>();
    }

    public override void Action(Token target) {
        Debug.Log("Sorcerer Action");
        Character c = (Character)target;

        if (c.team != GameManager.currentTeam) {
            if (this.stamina >= ATTACK_COST && c.HP >= 0) {
                target.place.changeState("enemy");
                this.FrozenAttack(c, minRange, maxRange);
            } else {
                Debug.Log("Not enough stamina");
            }
        } else {
            locked = false;
            target.Select();
        }

        if (stamina == 0) {
            locked = false;
        }
    }
    public void FrozenAttack(Character target, int minRange, int maxRange) {
        Debug.Log("Frozen Distance Attack");
        if (checkRange(minRange, maxRange, target.place)) {
            this.stamina -= FREEZE_COST;
            FrozenGridEffect ice = target.place.addEffect("frozen") as FrozenGridEffect;
            ice.user = this;
            ice.SpawnIce(target.place, bullet);
            frozenBlocks.Add(ice);
        }
    }
}
