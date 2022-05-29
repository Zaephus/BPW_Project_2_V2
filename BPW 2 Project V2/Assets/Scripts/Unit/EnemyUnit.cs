using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyUnit",menuName = "Unit/EnemyUnit")]
public class EnemyUnit : Unit {

    public GameObject unitPrefab;
    public List<Ability> abilities = new List<Ability>();

}