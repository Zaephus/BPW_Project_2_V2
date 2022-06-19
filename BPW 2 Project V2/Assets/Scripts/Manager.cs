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

    public void Start() {}

    public void Update() {}

    public IEnumerator StartBattle(EnemyUnit unit) {
        battleSystem.gameObject.SetActive(true);
        dungeon.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        battleSystem.Initialize(unit);
    }

    public IEnumerator WonBattle(EnemyController enemy) {
        
        battleSystem.gameObject.SetActive(false);

        Destroy(enemy.gameObject);
        dungeon.enemies.Remove(enemy);

        yield return new WaitForSeconds(1f);
        yield return new WaitForEndOfFrame();

        dungeon.gameObject.SetActive(true);
    }

    public IEnumerator LostBattle() {
        yield return null;
    }

    public void NextDungeon() {
        dungeon.StartCoroutine(dungeon.ResetDungeon());
    }

}