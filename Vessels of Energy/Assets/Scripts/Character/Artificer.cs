using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artificer : Character
{
    void Start()
    {
        //Stats still need to be balanced
        dexterity = 1;
        strength = 0;
        vitality = 2;
        intelligence = 3;
        perception = 4;
        willpower = 2;

        this.calculateStats();
    }

    public override void Action(){
        Debug.Log("Artificer Action");

        /*if(stamina == 0){
            locked = false;
        }*/
    }
}
