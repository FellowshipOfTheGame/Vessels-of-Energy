using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public QTE qte;
    public SceneLoader sceneLoader;

    public List<Character> unitsList;

    public static Team currentTeam;
    [HideInInspector] public bool endTurn;
    [HideInInspector] public bool newTurn;

    public VictoryScreen victoryScreen;
    public Team[] teams;

    class Crew {
        public Team team;
        public List<Character> members;
        public Crew(Team team) {
            this.team = team;
            members = new List<Character>();
        }
    }

    List<Crew> crews;


    private void Awake() {
        if (instance != null && instance != this) Destroy(this.gameObject);
        else instance = this;

        //setting things up
        Gyroscope.cam = Camera.main.transform;
        crews = new List<Crew>();
        foreach (Team t in teams) {
            Crew crew = new Crew(t);
            crews.Add(crew);

            foreach (Character c in unitsList) {
                if (c.team == t) crew.members.Add(c);
            }
        }
    }

    void Start() { ResetGame(); }
    public void ResetGame() { StartTurn(teams[0]); }

    public void StartTurn(Team t) {
        currentTeam = t;
        foreach (HexGrid hex in GridManager.arena) hex.OnTurnStart();
    }

    public void EndTurn() {
        //call all end turn events
        foreach (HexGrid hex in GridManager.arena) hex.OnTurnEnd();

        //clean up everything
        if (Token.selected != null) Token.selected.Unselect();
        if (Character.target != null) Character.target.Unselect();
        HUDManager.instance.Clear();

        //calculate next team
        for (int i = 0; i < teams.Length; i++) {
            if (teams[i] == currentTeam) {
                int next = (i + 1) % teams.Length;
                StartTurn(teams[next]);
                return;
            }
        }
    }

    void Update() {
        //Presing space ends the current turn
        if (Input.GetKeyDown(KeyCode.Space)) EndTurn();
    }

    public void CheckEndGame() {
        Crew winners = getWinners();

        if (winners != null) {
            Debug.Log(winners.team.name + " is the Winner!");
            victoryScreen.DisplayTeam(winners.members);
        } else {
            Debug.Log("No Winner Yet...");
        }
    }

    //Checks if there is a winner after each attack/counter
    Crew getWinners() {
        Crew winner = null;

        foreach (Crew crew in crews) {
            bool dead = true;
            foreach (Character c in crew.members) {
                if (c.HP > 0) {
                    dead = false;
                    break;
                }
            }

            if (!dead) {
                if (winner == null)
                    winner = crew;
                else
                    return null;
            }
        }

        //suport for future draws
        return winner;
    }
}
