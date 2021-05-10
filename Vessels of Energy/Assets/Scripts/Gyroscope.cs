using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyroscope : MonoBehaviour {
    class Object2D {
        public Renderer renderer;
        public int order;

        public Object2D(Renderer renderer) {
            this.renderer = renderer;
            this.order = renderer.sortingOrder;
        }
    }

    List<Object2D> objects;
    Transform cam;
    int margin = 40;
    public int order = 0;
    public List<Renderer> renderers;


    // Start is called before the first frame update
    void Start() {
        cam = Camera.main.transform;
        objects = new List<Object2D>();

        renderers = new List<Renderer>();
        renderers = GetAllRenderers(this.transform, renderers);
        foreach (Renderer r in renderers) {
            objects.Add(new Object2D(r));
        }

        /*
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)  objects.Add(new Object2D(sprite));

        TrailRenderer[] trails = GetComponentsInChildren<TrailRenderer>();
        foreach (TrailRenderer trail in trails) objects.Add(new Object2D(trail)); */
    }

    // Update is called once per frame
    void Update() {
        this.transform.forward = new Vector3(cam.forward.x, this.transform.forward.y, cam.forward.z);
        foreach (Object2D obj in objects) {
            int distance = (int)((cam.position - this.transform.position).magnitude * 10f);
            distance = Mathf.Clamp(distance, 0, 300);
            obj.renderer.sortingOrder = obj.order + (300 - distance) * margin + order * margin / 2;
        }
    }

    List<Renderer> GetAllRenderers(Transform t, List<Renderer> list) {
        for (int i = 0; i < t.childCount; i++) {
            Transform child = t.GetChild(i);
            Renderer r = child.gameObject.GetComponent<Renderer>();
            if (r != null) list.Add(r);
            GetAllRenderers(child, list);
        }

        return list;
    }
}
