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
        yield return null;
    }
}