using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcerer : Character
{
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

    public override void Action(){
        Debug.Log("Sorcerer Action");

        /*if(stamina == 0){
            locked = false;
        }*/
    }

}
