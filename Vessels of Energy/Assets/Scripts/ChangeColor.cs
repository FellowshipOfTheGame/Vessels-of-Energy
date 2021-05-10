using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour {

    [System.Serializable]
    public class TeamColor {
        public string name;
        public Color[] color;
    }

    [System.Serializable]
    public class SpriteMesh {
        public string name;
        public SpriteRenderer[] pieces;
    }

    public TeamColor[] colors;
    
    
    public SpriteMesh[] mesh;

    // Start is called before the first frame update
    public void UpdateColors(int team) {
        for(int i = 0; i < mesh.Length; i++) {
            foreach (SpriteRenderer piece in mesh[i].pieces) {
                piece.color = colors[team].color[i];
            }
        }        
    }
}