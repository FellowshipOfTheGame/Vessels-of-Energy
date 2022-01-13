using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Team", fileName = "New Team")]
public class Team : ScriptableObject {
    new public string name;
    public Color[] color;
}
