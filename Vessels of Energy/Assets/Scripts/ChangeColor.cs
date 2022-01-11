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
        if (team >= colors.Length) return;

        for(int i = 0; i < mesh.Length; i++) {
            foreach (SpriteRenderer piece in mesh[i].pieces) {
                if (i >= colors[team].color.Length) {
                    Debug.Log(colors[team].name + " brush doesn't have a color " + i.ToString());
                    break;
                }
                piece.color = colors[team].color[i];
            }
        }
    }

    public Color GetColor(char team) {
        switch (team) {
            case 'A':
                return GetColor(0);
            case 'B':
                return GetColor(1);
            case 'C':
                return GetColor(2);
            case 'D':
                return GetColor(3);
        }

        return Color.black;
    }

    public Color GetColor(int team) {
        if (team < colors.Length) return colors[team].color[0];
        else return Color.black;
    }
}
