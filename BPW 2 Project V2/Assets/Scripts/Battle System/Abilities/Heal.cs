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
        yield return null;
    }
    
}