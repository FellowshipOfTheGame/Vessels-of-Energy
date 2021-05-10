using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue : Character
{
    void Start()
    {
        //Stats still need to be balanced
        dexterity = 4;
        strength = 2;
        vitality = 1;
        intelligence = 1;
        perception = 3;
        willpower = 1;

        this.calculateStats();
    }

    public override void Action(){
        Debug.Log("Rogue Action");

        /*if(stamina == 0){
            locked = false;
        }*/
    }

}
