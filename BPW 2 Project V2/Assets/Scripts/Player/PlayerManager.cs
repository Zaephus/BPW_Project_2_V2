using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    private DungeonManager dungeon;

    public PlayerUnit unit;
    public GameObject inventoryObject;
    public RectTransform inventory;
    private bool onInventory = false;

    private PlayerController controller;
    public PlayerInteract interact;

    public void OnStart() {

        dungeon = FindObjectOfType<DungeonManager>();

        controller = GetComponent<PlayerController>();
        controller.Initialize(dungeon);
        interact.OnStart(this);

    }

    public void OnUpdate() {

        if(Input.GetButtonDown("Inventory")) {
            onInventory = !onInventory;
        }

        if(onInventory) {
            inventoryObject.SetActive(true);
        }
        else {
            inventoryObject.SetActive(false);
        }
        
        controller.OnUpdate();
        interact.OnUpdate();
    }

}