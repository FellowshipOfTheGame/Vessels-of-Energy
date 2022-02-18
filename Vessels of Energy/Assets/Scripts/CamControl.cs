using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour {

    class Configuration {
        public Vector3 pos, axis;
        public Quaternion rot;
        public float zoom;

        public Configuration(Vector3 pos, Vector3 axis, float zoom) {
            this.pos = pos;
            this.axis = axis;
            this.rot = Quaternion.FromToRotation(Vector3.forward, axis);
            this.rot.eulerAngles = new Vector3(this.rot.eulerAngles.x, this.rot.eulerAngles.y, 0f);
            this.zoom = zoom;
        }
    }


    public static CamControl instance;
    Camera cam;
    Configuration defaultConfig;
    Transform pivot;
    bool onFocus;

    public float zoomIncrement = 0.02f;
    public float zoom = 0.2f, delay = 0.5f, tolerance = 0.1f;
    public float height = 0.19f;
    [Space(5)]
    public float offsetY = 0.03f;
    public float offsetZ = 0.4f;
    float offset;

    private void Awake() {
        if (instance != null && instance != this) Destroy(this.gameObject);
        else instance = this;

        cam = this.GetComponent<Camera>();
        offset = (offsetY * Vector3.up - offsetZ * Vector3.forward).magnitude;
        defaultConfig = new Configuration(this.transform.position + this.transform.forward * offset, this.transform.forward, cam.orthographicSize);
        //Debug.Log(defaultConfig.rot.eulerAngles);
        pivot = this.transform.GetChild(0);
        pivot.localPosition = this.transform.forward * offset;
        onFocus = false;
    }

    // Update is called once per frame
    public void Shift(Vector3 translation, Vector3 rotation) {
        if (onFocus) return;

        //calculating cam forward based on destiny and direction
        Vector3 pos = defaultConfig.pos + translation;
        Vector3 axis = Quaternion.Euler(rotation) * defaultConfig.axis;
        defaultConfig = new Configuration(pos, axis, defaultConfig.zoom);

        this.transform.position = pos - offset * axis;
        this.transform.rotation = defaultConfig.rot;
    }

    public void Focus(params Transform[] target) {
        if (target.Length == 0) return;


        //getting most distant targets
        Vector3 focus1 = Vector3.zero, focus2 = Vector3.zero;
        for (int i = 0; i < target.Length - 1; i++) {
            for (int j = i; j < target.Length; j++) {
                if ((focus1 - focus2).magnitude <= (target[i].position - target[j].position).magnitude) {
                    focus1 = target[i].position;
                    focus2 = target[j].position;
                }
            }
        }

        //calculate the camera plane and zoom
        Vector3 center = (focus1 + focus2) / 2f;
        Vector3 direction = Quaternion.Euler(0f, 90f, 0f) * (focus1 - focus2).normalized;
        float frame = zoom + zoomIncrement * (focus1 - focus2).magnitude / 0.166f;
        CamControl.instance.Focus(center, direction);

        //calculating cam forward based on center and direction
        Vector3 pos = center + height * Vector3.up;
        pivot.transform.position = pos + offsetY * Vector3.up - offsetZ * direction;
        pivot.LookAt(pos);

        //starting process
        onFocus = true;
        StartCoroutine(Move(new Configuration(pos, pivot.forward, frame), delay, () => { }));
    }

    public void Focus(Vector3 destiny, Vector3 direction) {
        onFocus = true;
        //calculating cam forward based on destiny and direction
        Vector3 pos = destiny + height * Vector3.up;
        pivot.transform.position = pos + offsetY * Vector3.up - offsetZ * direction;
        pivot.LookAt(pos);

        StartCoroutine(Move(new Configuration(pos, pivot.forward, zoom), delay, () => { }));
    }

    public void Unfocus() {
        StartCoroutine(Move(defaultConfig, delay, () => { onFocus = false; }));
    }

    IEnumerator Move(Configuration destiny, float delay, System.Action onStop) {
        Configuration start = new Configuration(this.transform.position + this.transform.forward * offset, this.transform.forward, cam.orthographicSize);
        Configuration end = new Configuration(destiny.pos, destiny.axis, destiny.zoom);
        float timeElapsed = 0;

        float travelDistance = (end.pos - start.pos).magnitude;
        float focusDistance = (destiny.pos - start.pos).magnitude;

        while (timeElapsed < delay) {
            float step = timeElapsed / delay;
            float smooth = step * step * (3f - 2f * step); //smooth curve (x = 3t^2 - 2t^3)
            float linear = step; //linear curve (x = t)

            pivot.transform.position = Vector3.Lerp(start.pos, end.pos, linear);
            this.transform.rotation = Quaternion.Lerp(start.rot, end.rot, smooth);
            this.transform.position = pivot.transform.position - this.transform.forward * offset;
            cam.orthographicSize = Mathf.Lerp(start.zoom, end.zoom, linear);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        this.transform.position = end.pos - offset * end.axis;
        this.transform.rotation = end.rot;
        cam.orthographicSize = end.zoom;
        onStop.Invoke();
    }
}
