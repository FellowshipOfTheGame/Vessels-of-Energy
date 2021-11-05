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
        //Stats still need to be balanced
        dexterity = 1;
        strength = 0;
        vitality = 1;
        intelligence = 4;
        perception = 2;
        willpower = 3;

        this.calculateStats();
    }

    public override void Action()
    {
        Debug.Log("Sorcerer Action");

        if (target.team != GameManager.currentTeam)
        {
            if (this.stamina >= ATTACK_COST && target.HP >= 0)
            {
                target.place.changeState("enemy");
                this.MediumDistanceAttack(target, minRange, maxRange);
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
}
