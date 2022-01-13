using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNoiseGenerator : MonoBehaviour {

    public float delay;
    [Range(0f, 1f)]
    public float randomness;
    new AudioManager audio;
    public bool playing;

    // Start is called before the first frame update
    void Start() {
        playing = true;
        audio = GetComponent<AudioManager>();
        StartCoroutine(Play());
    }


    IEnumerator Play() {
        while (playing && audio.sounds.Length > 0) {
            float delay = this.delay * (1f + randomness * Random.Range(-1f, 1f));
            yield return new WaitForSeconds(delay);

            int index = Random.Range(0, audio.sounds.Length - 1);
            audio.Play(audio.sounds[index].name);
        }
    }
}
