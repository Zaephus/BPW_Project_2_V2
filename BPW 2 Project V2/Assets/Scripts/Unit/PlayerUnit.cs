using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUnit",menuName = "Unit/PlayerUnit")]
public class PlayerUnit : Unit {

    public List<Ability> abilities = new List<Ability>();

}