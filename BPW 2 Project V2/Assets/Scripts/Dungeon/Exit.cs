using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour,IInteractable {
    public void Interact(PlayerManager p) {
        Manager.instance.NextDungeon();
    }
}