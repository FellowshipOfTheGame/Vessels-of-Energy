using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour {
    public GameObject[] objects;
    List<Renderer> renderers;
    bool opaque = true;
    public float delayIn = 0.1f, delayOut = 0.3f, alpha = 0.3f;
    public AnimationCurve fadeIn, fadeOut;

    void Awake() {
        opaque = true;
        renderers = new List<Renderer>();

        foreach (GameObject obj in objects) {
            Renderer[] meshes = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer mesh in meshes) renderers.Add(mesh);
        }

    }

    public void FadeIn() {
        if (opaque) return;
        Debug.Log(name + " Fading In...");

        opaque = true;
        StopAllCoroutines();
        StartCoroutine(FadeAllMaterials(alpha, 1f, delayIn, () => {
            foreach (Renderer renderer in renderers) {
                foreach (Material material in renderer.materials) {
                    //set material to opaque mode
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
                }
            }
        }));
    }


    public void FadeOut() {
        if (!opaque) return;
        Debug.Log(name + "Fading Out...");

        opaque = false;
        foreach (Renderer renderer in renderers) {
            foreach (Material material in renderer.materials) {
                //allows material to fade mode
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.EnableKeyword("_ALPHABLEND_ON");
                material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            }
        }

        StopAllCoroutines();
        StartCoroutine(FadeAllMaterials(1f, alpha, delayOut, () => { }));
    }


    IEnumerator FadeAllMaterials(float startAlpha, float endAlpha, float delay, System.Action OnStop) {
        float timeElapsed = 0;

        while (timeElapsed < delay) {
            float step = timeElapsed / delay;
            float fade = step;

            if (startAlpha < endAlpha) fade = fadeIn.Evaluate(step);
            else fade = fadeOut.Evaluate(step);

            foreach (Renderer renderer in renderers) {
                foreach (Material material in renderer.materials) {
                    material.color = new Color(
                        material.color.r,
                        material.color.g,
                        material.color.b,
                        Mathf.Lerp(startAlpha, endAlpha, fade)
                    );
                }
            }

            timeElapsed += Time.deltaTime;
            yield return new WaitForSeconds(delay / 10f);
        }

        OnStop.Invoke();
    }
}
