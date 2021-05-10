using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCollider : MonoBehaviour{

    [HideInInspector] public RaycastTarget target;

    public void OnPointerEnter() { 
        target.OnRaycastEnter(this);
    }

    public void OnPointerExit() {
        target.OnRaycastExit(this);
    }

    public void OnClick(int mouseButton) {
        target.OnRaycastClick(this, mouseButton);
    }
}
