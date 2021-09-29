using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue : Character
{
    void Start()
    {
        this.stats.calculateStats();
        this.HP = stats.maxHP;
    }

    public override void Action(){
        Debug.Log("Rogue Action");

        /*if(stamina == 0){
            locked = false;
        }*/
    }

}
