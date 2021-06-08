using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public QTE qte;
    public SceneLoader sceneLoader;

    public List<Character> unitsList;
    [HideInInspector] public List<Character> teamAList;
    [HideInInspector] public List<Character> teamBList;

    public static char currentTeam;
    [HideInInspector] public bool endTurn;
    [HideInInspector] public bool newTurn;

    public VictoryScreen victoryScreen;

    private void Awake () {
        if ( instance != null && instance != this )
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        teamAList = new List<Character>();
        teamBList = new List<Character>();

        //Get Character's team and add it to its team List
        foreach (Character c in unitsList)
        {
            if (c.team == 'A')
            {
                teamAList.Add(c);
            }
            else if (c.team == 'B')
            {
                teamBList.Add(c);
            }
        }

        currentTeam = 'A';
        newTurn = true;
        Gyroscope.cam = Camera.main.transform;
    }

    void Update()
    {
        //At the start of a new turn
        if (newTurn) {
            resetStamina();
            newTurn = false;
            HUDManager.instance.Clear();
            foreach (Character unit in unitsList) unit.place.OnTurnStart();
        }

        if (endTurn) {
            if ( Token.selected != null ) {
                Token.selected.Unselect();
            }

            switch (currentTeam) {
                case 'A':
                    currentTeam = 'B';
                    break;
                case 'B':
                    currentTeam = 'A';
                    break;
                default:
                    Debug.Log("Team does not exist");
                    break;
            }
            newTurn = true;
            endTurn = false;
        }

        //Presing space ends the current turn
        if (Input.GetKeyDown(KeyCode.Space))
            endTurn = true;

    }

    public void resetStamina()
    {
        if (currentTeam == 'A')
        {
            for (int i = 0; i < teamAList.Count; i++)
            {
                teamAList[i].resetStamina();
            }
        }
        else
        {
            for (int i = 0; i < teamBList.Count; i++)
            {
                teamBList[i].resetStamina();
            }
        }
    }

    //Checks if there is a winner after each attack/counter
    public void checkWinner() {

        //If all characters in team A are dead, B is the winner
        bool endGame = true;
        foreach (Character c in teamAList){
            if ( c.HP > 0 ) {
                endGame = false;
                break;
            }
        }
        if (endGame) {
            Debug.Log("B is the Winner");
            victoryScreen.DisplayTeam(teamBList);
            return;
        }

        //If all characters in team B are dead, A is the winner
        endGame = true;
        foreach (Character c in teamBList){
            if ( c.HP > 0 ) {
                endGame = false;
                break;
            }
        }
        if ( endGame ) {
            Debug.Log("A is the Winner");
            victoryScreen.DisplayTeam(teamAList);
            return;
        }

        Debug.Log("There is no winner yet...");
    }
}
