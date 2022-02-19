using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamOcclusion : MonoBehaviour {

    public Transform[] targets;
    public Transform mouse;

    List<Fade> detected, lastDetected;

    void Awake() {
        mouse = null;
        detected = new List<Fade>();
        lastDetected = new List<Fade>();
    }

    public void Scan() {
        detected = new List<Fade>();
        foreach (Transform t in targets) ScanOcclusion(t.position, true);
        if (mouse != null) ScanOcclusion(mouse.position, false);

        foreach (Fade f in lastDetected) {
            if (!detected.Contains(f))
                f.FadeIn();
        }

        foreach (Fade f in detected) {
            Debug.Log(f.name + " detected!");
            if (!lastDetected.Contains(f)) {
                Debug.Log(f.name + ", go away!");
                f.FadeOut();
            }
        }

        lastDetected = detected;
    }

    void ScanOcclusion(Vector3 focus, bool projectPos) {
        // calculating reference position
        Vector3 myPos = this.transform.position;
        if (projectPos) myPos = new Vector3(this.transform.position.x, focus.y, this.transform.position.z);

        // setting up rays
        Vector3 distance = focus - myPos;
        Ray forward = new Ray(focus, -this.transform.forward);
        Ray backward = new Ray(focus, this.transform.forward);
        Debug.DrawRay(forward.origin, forward.direction * 5, Color.red);
        Debug.DrawRay(backward.origin, backward.direction * 5, Color.red);

        //getting all hits
        List<RaycastHit> hits = new List<RaycastHit>();
        hits.AddRange(Physics.RaycastAll(forward, 5f));
        hits.AddRange(Physics.RaycastAll(backward, 5f));
        foreach (RaycastHit hit in hits) {
            Fade fade = hit.transform.gameObject.GetComponent<Fade>();
            if (fade != null) detected.Add(fade);
        }

    }
}
