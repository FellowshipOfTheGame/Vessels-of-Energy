using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcerer : Character
{
    private int minRange = 2, maxRange = 4;
    public GameObject bullet;
    //public ParticleSystem attackParticle;

    void Start()
    {
        this.stats.calculateStats();
        this.HP = stats.maxHP;
    }

    public override void Action()
    {
        Debug.Log("Sorcerer Action");

        if (target.team != GameManager.currentTeam)
        {
            if (this.stamina >= ATTACK_COST && target.HP >= 0)
            {
                target.place.changeState("enemy");
                this.FrozenAttack(target, minRange, maxRange);
            }
            else
            {
                Debug.Log("Not enough stamina");
            }
        }
        else
        {
            locked = false;
            target.Select();
        }

        if (stamina == 0)
        {
            locked = false;
        }
    }
    public void MediumDistanceAttack(Character target, int minRange, int maxRange)
    {
        Debug.Log("Medium Distance Attack");
        if (checkRange(minRange, maxRange))
        {
            this.stamina -= Character.ATTACK_COST;
            target.HP -= Character.ATTACK_COST;
        }
    }
    public void FrozenAttack(Character target, int minRange, int maxRange)
    {
        Debug.Log("Frozen Distance Attack");
        if (checkRange(minRange, maxRange))
        {
            this.stamina -= Character.ATTACK_COST;
            target.isFrozen = true;
            target.place.changeState("frozen");

        }
    }
}
