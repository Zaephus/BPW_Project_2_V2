using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour,IInteractable {

    private PlayerManager player;

    public GameObject UI_Item;

    public string itemName;

    public void Interact(PlayerManager p) {
        player = p;
        player.unit.items.Add(this);
        GameObject tempItem = Instantiate(UI_Item,player.inventory);
        //tempItem.transform.SetParent(player.inventory,false);

        Destroy(this.gameObject);
    }

}