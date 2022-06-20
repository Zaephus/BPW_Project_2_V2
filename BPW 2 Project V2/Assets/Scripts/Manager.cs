using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    #region Singleton
    public static Manager instance;

    void Awake() {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion;

    public DungeonManager dungeon;
    public BattleSystem battleSystem;
    public GameObject mainMenu;
    public GameObject gameOverMenu;
    private SaveSystem saveSystem;

    public PlayerUnit pUnit;

    public List<Item> items = new List<Item>();

    public void Start() {
        saveSystem = GetComponent<SaveSystem>();
    }

    public void Update() {
        if(Input.GetKeyDown("escape")) {
            StartCoroutine(MainMenu());
        }
    }

    public IEnumerator MainMenu() {
        yield return new WaitForEndOfFrame();
        battleSystem.gameObject.SetActive(false);
        dungeon.gameObject.SetActive(false);
        gameOverMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void StartGame() {

        // pUnit.currentAttackStrength = pUnit.baseAttackStrength;
        // pUnit.currentDefenseStrength = pUnit.baseDefenseStrength;
        // pUnit.currentHealth = pUnit.maxHealth;
        // pUnit.items.Clear();

        saveSystem.LoadUnit(pUnit,"PlayerUnit");

        battleSystem.gameObject.SetActive(false);
        gameOverMenu.SetActive(false);
        mainMenu.SetActive(false);
        dungeon.gameObject.SetActive(true);

    }

    public IEnumerator StartBattle(EnemyUnit unit) {
        battleSystem.gameObject.SetActive(true);
        dungeon.gameObject.SetActive(false);
        gameOverMenu.SetActive(false);
        mainMenu.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        battleSystem.Initialize(unit);
    }

    public IEnumerator WonBattle(EnemyController enemy) {
        
        battleSystem.gameObject.SetActive(false);
        gameOverMenu.SetActive(false);
        mainMenu.SetActive(false);

        Instantiate(items[Random.Range(0,items.Count)],enemy.transform.position,Quaternion.identity,enemy.transform.parent);

        Destroy(enemy.gameObject);
        dungeon.enemies.Remove(enemy);

        yield return new WaitForSeconds(1f);
        yield return new WaitForEndOfFrame();

        dungeon.gameObject.SetActive(true);
    }

    public IEnumerator LostBattle() {
        gameOverMenu.SetActive(true);
        pUnit.currentHealth = pUnit.maxHealth;
        yield return new WaitForEndOfFrame();
        
        battleSystem.gameObject.SetActive(false);
        dungeon.gameObject.SetActive(false);
        mainMenu.SetActive(false);
    }

    public void NextDungeon() {
        dungeon.StartCoroutine(dungeon.ResetDungeon());
    }

    public void Exit() {
        saveSystem.SaveUnit(pUnit,"PlayerUnit");
        Application.Quit();
    }

}