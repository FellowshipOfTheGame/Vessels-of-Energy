using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTarget : MonoBehaviour {
    
    protected bool highlight = false;
    int hitCount = 0;

    public virtual void Awake() {
        highlight = false;
        hitCount = 0;

        RaycastCollider[] colliders = GetComponentsInChildren<RaycastCollider>();
        foreach(RaycastCollider c in colliders) c.target = this;
    }

    public void OnRaycastEnter(RaycastCollider collider) {
        if (!highlight) {
            OnPointerEnter();
            highlight = true;
        }
        hitCount++;
    }

    public void OnRaycastExit(RaycastCollider collider) {
        hitCount--;
        if(highlight && hitCount <= 0){
            OnPointerExit();
            highlight = false;
        }
    }

    public void OnRaycastClick(RaycastCollider collider, int mouseButton) {
        if (highlight){
            OnClick(mouseButton);
        }
    }


    public virtual void OnPointerEnter(){}

    public virtual void OnPointerExit(){}

    public virtual void OnClick(int mouseButton){}

    
}