using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamInput : MonoBehaviour {

    public float speed = 10f;
    [Space(5)]
    public Vector3 lowerLimit = - Vector3.one * 10f;
    public Vector3 upperLimit = Vector3.one * 10f;

    CamControl control;
    Vector3 axis;
    public static bool block = false;

    // Start is called before the first frame update
    void Start() {
        control = this.GetComponent<CamControl>();
        axis = (new Vector3(this.transform.forward.x, 0.0f, this.transform.forward.z)).normalized;
    }

    // Update is called once per frame
    void Update(){
        if (block) return;

        if (Input.GetKey(KeyCode.W)) {
            Vector3 translation = axis * speed * Time.deltaTime;
            control.Shift(ClampOffset(translation), Vector3.zero);
        } else if (Input.GetKey(KeyCode.A)) {
            Vector3 translation = Quaternion.Euler(0f, -90f, 0f) * axis * speed * Time.deltaTime;
            control.Shift(ClampOffset(translation), Vector3.zero);
        } else if (Input.GetKey(KeyCode.S)) {
            Vector3 translation = - axis * speed * Time.deltaTime;
            control.Shift(ClampOffset(translation), Vector3.zero);
        } else if (Input.GetKey(KeyCode.D)) {
            Vector3 translation = Quaternion.Euler(0f, 90f, 0f) * axis * speed * Time.deltaTime;
            control.Shift(ClampOffset(translation), Vector3.zero);
        }
    }

    Vector3 ClampOffset(Vector3 offset) {
        Vector3 destiny = this.transform.position + offset;

        destiny.x = Mathf.Clamp(destiny.x, lowerLimit.x, upperLimit.x);
        destiny.y = Mathf.Clamp(destiny.y, lowerLimit.y, upperLimit.y);
        destiny.z = Mathf.Clamp(destiny.z, lowerLimit.z, upperLimit.z);

        return destiny - this.transform.position;
    }
}
