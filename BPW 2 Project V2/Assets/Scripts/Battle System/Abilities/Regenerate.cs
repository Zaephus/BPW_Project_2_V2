using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Regenerate",menuName = "Abilities/Regenerate")]
public class Regenerate : Ability {

    private BattleSystem battleSystem;

    public int healAmount;

    public override void Initialize(BattleSystem b) {
        battleSystem = b;
    }

    public override IEnumerator DoBehaviour() {

        if(battleSystem.enemyUnit.currentHealth < battleSystem.enemyUnit.maxHealth) {

            battleSystem.StartCoroutine(battleSystem.TypeWriter(battleSystem.enemyUnit.unitName + " regenerates!"));
            yield return new WaitUntil(() => battleSystem.dialogueActivated == false);

            battleSystem.enemyUnit.Heal(healAmount);
            battleSystem.enemyHUD.SetHealth(battleSystem.enemyUnit.currentHealth);

            battleSystem.state = BattleState.Wait;
            battleSystem.StartCoroutine(battleSystem.PlayerTurn());

        }
        else {
            battleSystem.StartCoroutine(battleSystem.EnemyTurn());
        }

    }
    
}