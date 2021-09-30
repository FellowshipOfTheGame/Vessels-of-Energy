using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryScreen : MonoBehaviour {

    public TextMeshProUGUI teamLabel;
    public Image teamHeader, teamFocus;
    [Space(5)]
    public GameObject victoryCam;
    public List<HexGrid> victorySpot;

    List<Character> winners;
    Animator animator;
    AudioManager audio;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        audio = this.GetComponent<AudioManager>();
        audio.Play("music");
        victoryCam.SetActive(false);
        foreach (HexGrid hex in victorySpot) {
            hex.art.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayTeam(List<Character> winners) { //called by any script
        this.winners = winners;
        Raycast.block = true;

       /*ChangeColor pallete = winners[0].animator.GetComponent<ChangeColor>();
       switch (winners[0].team) {
            case 'A':
                teamHeader.color = pallete.colors[0].color[0];
                teamFocus.color = pallete.colors[0].color[0];
                teamLabel.outlineColor = pallete.colors[0].color[0];
                teamLabel.text = "VITÓRIA!";
                break;

            case 'B':
                teamHeader.color = pallete.colors[1].color[0];
                teamFocus.color = pallete.colors[1].color[0];
                teamLabel.outlineColor = pallete.colors[1].color[0];
                teamLabel.text = "VITÓRIA!";
                break;
        }*/
        Color color = animator.GetComponent<ChangeColor>().GetColor(winners[0].team);
        teamHeader.color = color;
        teamFocus.color = color;
        teamLabel.outlineColor = color;
        teamLabel.text = "VITÓRIA!";

        animator.SetBool("show", true);
    }

    public void Show() { //called by animation only!
        Gyroscope.cam = victoryCam.transform;
        victoryCam.SetActive(true);
        CamControl.instance.gameObject.SetActive(false);
        HUDManager.instance.Clear();

        int index = 0;
        foreach (Character c in winners) {
            if (c.HP > 0) {
                c.Move(victorySpot[index]);
                c.animator.Dance();
                index++;
            }
        }
    }
}
