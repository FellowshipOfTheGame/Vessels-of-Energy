using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCover : Prop {
    [HideInInspector] public FrozenGridEffect effect;

    public override void OnRemoved() {
        Debug.Log("Breaking the Ice");
        //effect.Cancel();
    }
}
