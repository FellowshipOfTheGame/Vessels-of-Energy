using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prop", fileName = "New Prop")]
public class PropProperty : ScriptableObject {
    public bool breakable;
    public int size;

    [Space(5)]
    public bool throwable;
}
