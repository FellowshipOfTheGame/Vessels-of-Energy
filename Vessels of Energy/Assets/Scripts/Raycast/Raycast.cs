using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Raycast : MonoBehaviour {
    public static bool block = false;
    RaycastCollider lastHit = null;
    public LayerMask interactableLayer = 1 << 8;

    void Start() {
        lastHit = null;
    }

    void Update() {
        if (block || EventSystem.current.IsPointerOverGameObject()) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.cyan);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer)) {
            RaycastCollider target = hit.transform.GetComponent<RaycastCollider>();
            if (target != null && lastHit != target) {
                if (lastHit != null) lastHit.OnPointerExit();
                target.OnPointerEnter();
                lastHit = target;
            }
        } else {
            if (lastHit != null) {
                lastHit.OnPointerExit();
                lastHit = null;
            }
        }

        if (lastHit != null && Input.GetMouseButtonDown(0)) lastHit.OnClick(0);
        if (lastHit != null && Input.GetMouseButtonDown(1)) lastHit.OnClick(1);
    }
}
