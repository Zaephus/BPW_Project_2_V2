using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tackle",menuName = "Abilities/Tackle")]
public class Tackle : Ability {

    private BattleSystem battleSystem;

    public int basePower;

    public override void Initialize(BattleSystem b) {
        battleSystem = b;
    }

    public override IEnumerator DoBehaviour() {
        
        battleSystem.state = BattleState.Wait;

        battleSystem.StartCoroutine(battleSystem.TypeWriter("You tackle the enemy " + battleSystem.enemyUnit.unitName + "!"));
        yield return new WaitUntil(() => battleSystem.dialogueActivated == false);

        bool isDead = battleSystem.enemyUnit.TakeDamage(basePower*battleSystem.playerUnit.currentAttackStrength);
        battleSystem.enemyHUD.SetHealth(battleSystem.enemyUnit.currentHealth);

        if(isDead) {
            battleSystem.state = BattleState.Won;
            battleSystem.StartCoroutine(battleSystem.EndBattle());
        }
        else {
            battleSystem.state = BattleState.EnemyTurn;
            battleSystem.StartCoroutine(battleSystem.EnemyTurn());
        }

    }
    
}