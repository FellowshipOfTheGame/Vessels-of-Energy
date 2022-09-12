using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceRoller : MonoBehaviour {

    public static DiceRoller instance;

    public GameObject resultPrefab;
    public Sprite[] shapes;

    [Space(5)]
    public GameObject[] diceOnUI;

    bool rolling;
    Animator animator;


    public class Die {
        public Image type;
        public int size, value;
        public TextMeshProUGUI number;
        Sprite[] shapes;

        public Die(Image format, TextMeshProUGUI text, Sprite[] shapes) {
            type = format;
            size = 0;
            value = 0;
            number = text;
            this.shapes = shapes;
        }

        public void changeSize(int size) {
            this.size = size;
            type.sprite = shapes[size / 2 - 2];
            this.number.text = size.ToString();
        }
    }

    List<Die> dice;

    private void Awake() {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rolling = false;
        animator = this.GetComponent<Animator>();
        dice = new List<Die>();

        foreach (GameObject die in diceOnUI) {
            Image type = die.GetComponent<Image>();
            TextMeshProUGUI number = die.GetComponentInChildren<TextMeshProUGUI>();
            dice.Add(new Die(type, number, shapes));
        }
    }

    public int Roll(Character actor, params int[] dice) {

        int sum = 0;
        bool reroll = false;
        for (int i = 0; i < diceOnUI.Length; i++) {
            if (i < dice.Length) {
                diceOnUI[i].gameObject.SetActive(true);
                this.dice[i].type.color = actor.color;
                this.dice[i].changeSize(dice[i]);

                // Test
                if (i == 1){
                    this.dice[i].value = dice[i] + 1;
                }
                else{
                this.dice[i].value = Random.Range(1, dice[i] + 1);
                sum += this.dice[i].value;
                }
                // Test

                if (this.dice[i].value == dice[i] + 1 && actor.energy > 0){
                    QTE.instance.startQTE(QTE.Reaction.EXPLOSION, actor, () => {
                        Debug.Log(actor.Colored("EXPLOSION!"));
                        actor.energy -= 1;
                        reroll = true;
                        Debug.Log("Extra dice!");
                        },
                        () => { Debug.Log("Missed Opportunity to Explode..."); });
                }

                if(reroll){
                    reroll = false;
                    this.Roll(actor, dice[i]);
                }

            } else {
                diceOnUI[i].gameObject.SetActive(false);
            }
        }

        rolling = true;
        animator.SetBool("show", true);
        animator.SetTrigger("reset");

        StartCoroutine(RollNumbers());
        return sum;
    }

    public void ShowNumbers(string message) {
        rolling = false;
        StopAllCoroutines();

        int sum = 0;
        foreach(Die die in dice) {
            die.number.text = die.value.ToString();
            sum += die.value;
        }

        GameObject result = Instantiate(resultPrefab, this.transform);
        result.GetComponent<DiceResultUI>().Initialize(sum, message, dice[0].type.color);
    }

    public void Clear() {
        animator.SetBool("show", false);
    }

    IEnumerator RollNumbers() {
        while (rolling) {
            yield return new WaitForSeconds(0.1f);

            foreach(DiceRoller.Die die in dice ) {
                int value = Random.Range(1, die.size + 1);
                die.number.text = value.ToString();
            }
        }
    }
}
