using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Raycast : MonoBehaviour {
    public static bool block = false;
    RaycastCollider lastHit = null;
    public LayerMask interactableLayer = 1 << 8;

    CamOcclusion occlusion;

    void Awake() {
        lastHit = null;
        occlusion = this.GetComponent<CamOcclusion>();
    }

    void Start() {
        StopAllCoroutines();
        StartCoroutine(CastLoop());
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) PointerClick(lastHit, 0);
        if (Input.GetMouseButtonDown(1)) PointerClick(lastHit, 1);
        if (Input.GetMouseButtonDown(2)) PointerClick(lastHit, 2);
    }

    IEnumerator CastLoop() {
        while (true) {
            if (!block && !EventSystem.current.IsPointerOverGameObject()) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.cyan);
                Cast(ray);
            }

            occlusion.Scan();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Cast(Ray ray) {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer)) {
            occlusion.mouse = hit.transform;
            RaycastCollider target = hit.transform.GetComponent<RaycastCollider>();
            PointerEnter(target);
        } else {
            occlusion.mouse = null;
            PointerExit(lastHit);
        }
    }

    void PointerEnter(RaycastCollider target) {
        if (target != null && lastHit != target) {
            if (lastHit != null) lastHit.OnPointerExit();
            target.OnPointerEnter();
            lastHit = target;
        }
    }

    void PointerExit(RaycastCollider target) {
        if (target != null) {
            target.OnPointerExit();
            target = null;
        }
    }

    void PointerClick(RaycastCollider target, int index) {
        if (target != null) {
            target.OnClick(index);
        }
    }
}
