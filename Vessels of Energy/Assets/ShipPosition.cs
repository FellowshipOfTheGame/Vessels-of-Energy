using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPosition : MonoBehaviour
{
    Animator shipPos;

    void Start()
    {
        shipPos = GetComponent<Animator>();
    }

    public void PositionChange(int state)
    {
        shipPos.SetInteger("MenuState", state);
    }
}
