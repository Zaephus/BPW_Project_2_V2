using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Curse",menuName = "Abilities/Curse")]
public class Curse : Ability {

    private BattleSystem battleSystem;

    public int basePower;

    public override void Initialize(BattleSystem b) {
        battleSystem = b;
    }

    public override IEnumerator DoBehaviour() {
        yield return null;
    }
    
}