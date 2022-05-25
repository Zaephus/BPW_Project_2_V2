using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject {

    public string abilityName;

    public virtual void Initialize(BattleSystem battleSystem) {}
    public abstract IEnumerator DoBehaviour();
}