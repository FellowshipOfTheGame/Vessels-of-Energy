using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcerer : Character
{
    void Start()
    {
        this.stats.calculateStats();
        this.HP = stats.maxHP;
    }

    public override void Action(){
        Debug.Log("Sorcerer Action");

        /*if(stamina == 0){
            locked = false;
        }*/
    }

}
