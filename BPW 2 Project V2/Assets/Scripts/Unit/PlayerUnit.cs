using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUnit",menuName = "Unit/PlayerUnit")]
public class PlayerUnit : Unit {

    public List<Item> items = new List<Item>();

    public List<Ability> abilities = new List<Ability>();

}