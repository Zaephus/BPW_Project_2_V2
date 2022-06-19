using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal",menuName = "Abilities/Heal")]
public class Heal : Ability {

    private BattleSystem battleSystem;

    public int healAmount;

    public override void Initialize(BattleSystem b) {
        battleSystem = b;
    }

    public override IEnumerator DoBehaviour() {
        
        battleSystem.state = BattleState.Wait;

        if(battleSystem.playerUnit.currentHealth < battleSystem.playerUnit.maxHealth) {

            battleSystem.StartCoroutine(battleSystem.TypeWriter("You heal by " + healAmount + " HP!"));
            yield return new WaitUntil(() => battleSystem.dialogueActivated == false);
            
            battleSystem.playerUnit.Heal(healAmount);
            battleSystem.playerHUD.SetHealth(battleSystem.playerUnit.currentHealth);

            battleSystem.state = BattleState.EnemyTurn;
            battleSystem.StartCoroutine(battleSystem.EnemyTurn());

        }
        else {
            battleSystem.StartCoroutine(battleSystem.TypeWriter("You can not heal, you are already at full health!"));
            yield return new WaitUntil(() => battleSystem.dialogueActivated == false);
            battleSystem.state = BattleState.PlayerTurn;
        }
        
    }
    
}