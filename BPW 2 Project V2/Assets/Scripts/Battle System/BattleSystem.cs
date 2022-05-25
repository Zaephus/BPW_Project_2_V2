using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState {Start,PlayerTurn,EnemyTurn,Wait,Won,Lost}

public class BattleSystem : MonoBehaviour {

    public void Start() {}

    public void Update() {}

    public IEnumerator SetupBattle() {
        yield return null;
    }

    public IEnumerator PlayerTurn() {
        yield return null;
    }

    public IEnumerator EnemyTurn() {
        yield return null;
    }

    public IEnumerator EndBattle() {
        yield return null;
    }
}