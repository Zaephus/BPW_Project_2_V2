using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState {Start,PlayerTurn,EnemyTurn,Wait,Won,Lost}

public class BattleSystem : MonoBehaviour {

    public GameObject playerPrefab;
    private GameObject enemyPrefab;
    public GameObject abilityButtonPrefab;

    public Transform playerPosiiton;
    public Transform enemyPosition;

    public PlayerUnit playerUnit;
    [HideInInspector] public EnemyUnit enemyUnit;

    public TMP_Text dialogueText;

    public RectTransform combatButtons;
    [HideInInspector] public List<AttackButton> abilityButtons = new List<AttackButton>();

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    [HideInInspector] public BattleState state;

    [HideInInspector] public bool dialogueActivated;

    public void Initialize(EnemyUnit e) {

        dialogueText.text = "";

        enemyUnit = e;
        enemyPrefab = enemyUnit.unitPrefab;

        state = BattleState.Start;
        StartCoroutine(SetupBattle());

    }

    public void Update() {

        if(state == BattleState.PlayerTurn) {
            combatButtons.localScale = new Vector3(1,1,1);
        }
        else {
            combatButtons.localScale = new Vector3(0,0,0);
        }

    }

    public IEnumerator SetupBattle() {
        
        for(int i = 0; i < playerUnit.abilities.Count; i++) {
            GameObject abilityButton = Instantiate(abilityButtonPrefab,combatButtons.transform);

            abilityButtons.Add(abilityButton.GetComponent<AttackButton>());
            abilityButtons[i].Initialize(playerUnit.abilities[i],this);
        }

        foreach(Ability a in enemyUnit.abilities) {
            a.Initialize(this);
        }

        Instantiate(playerPrefab,playerPosiiton.position,Quaternion.identity,this.transform);
        Instantiate(enemyPrefab,enemyPosition.position,Quaternion.identity,this.transform);

        StartCoroutine(TypeWriter("A " + enemyUnit.unitName + " is attacking you!"));

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitUntil(() => dialogueActivated == false);

        state = BattleState.Wait;
        StartCoroutine(PlayerTurn());
    }

    public IEnumerator PlayerTurn() {
        
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(TypeWriter("Choose an action:"));
        yield return new WaitUntil(() => dialogueActivated == false);
        state = BattleState.PlayerTurn;

    }

    public IEnumerator EnemyTurn() {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(enemyUnit.abilities[Random.Range(0,enemyUnit.abilities.Count)].DoBehaviour());
    }

    public IEnumerator EndBattle() {

        foreach(AttackButton ab in abilityButtons) {
            Destroy(ab.gameObject);
        }
        abilityButtons.Clear();

        if(state == BattleState.Won) {
            StartCoroutine(TypeWriter("You won the battle!"));
            yield return new WaitUntil(() => dialogueActivated == false);
            yield return new WaitForSeconds(0.5f);
            Manager.instance.StartCoroutine(Manager.instance.WonBattle(enemyUnit.enemy));
        }
        else if(state == BattleState.Lost) {
            StartCoroutine(TypeWriter("You lost..."));
            yield return new WaitUntil(() => dialogueActivated == false);
            yield return new WaitForSeconds(0.5f);
            Manager.instance.StartCoroutine(Manager.instance.LostBattle());
        }

    }

    public IEnumerator TypeWriter(string dialogue) {

        dialogueText.text = "";
        dialogueActivated = true;

        foreach(char c in dialogue) {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.35f);
        dialogueActivated = false;
    }

}