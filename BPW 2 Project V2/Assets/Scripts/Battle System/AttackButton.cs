using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackButton : MonoBehaviour {

    public TMP_Text abilityName;
    private Ability ability;

    public void Initialize(Ability a,BattleSystem b) {
        
        ability = a;
        abilityName.text = a.abilityName;

        ability.Initialize(b);

    }

    public void DoBehaviour() {
        StartCoroutine(ability.DoBehaviour());
    }
}