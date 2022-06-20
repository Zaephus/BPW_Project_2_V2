using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShootSlime",menuName = "Abilities/ShootSlime")]
public class ShootSlime : Ability {

    private BattleSystem battleSystem;

    public int basePower;

    public override void Initialize(BattleSystem b) {
        battleSystem = b;
    }

    public override IEnumerator DoBehaviour() {

        battleSystem.StartCoroutine(battleSystem.TypeWriter(battleSystem.enemyUnit.unitName + " shoots slime at you!"));
        yield return new WaitUntil(() => battleSystem.dialogueActivated == false);

        bool isDead = battleSystem.playerUnit.TakeDamage(basePower*battleSystem.enemyUnit.currentAttackStrength);
        battleSystem.playerHUD.SetHealth(battleSystem.playerUnit.currentHealth);

        if(isDead) {
            battleSystem.state = BattleState.Lost;
            battleSystem.StartCoroutine(battleSystem.EndBattle());
        }
        else {
            battleSystem.state = BattleState.Wait;
            battleSystem.StartCoroutine(battleSystem.PlayerTurn());
        }

    }
    
}