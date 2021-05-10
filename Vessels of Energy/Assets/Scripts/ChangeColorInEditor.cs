using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChangeColor))]
[ExecuteInEditMode]
public class ChangeColorInEditor : MonoBehaviour {
    
    ChangeColor handler;
    public int team;
    private void OnValidate() {
        if (handler == null) handler = this.GetComponent<ChangeColor>();

        if (team < handler.colors.Length)
            handler.UpdateColors(team);
        else
            Debug.Log("Missing Team");
    }
}
