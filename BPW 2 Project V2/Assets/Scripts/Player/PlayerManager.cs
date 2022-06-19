using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    private DungeonManager dungeon;

    private PlayerController controller;
    public PlayerInteract interact;

    public void OnStart() {

        dungeon = FindObjectOfType<DungeonManager>();

        controller = GetComponent<PlayerController>();
        controller.Initialize(dungeon);
        interact.OnStart(this);

    }

    public void OnUpdate() {
        controller.OnUpdate();
        interact.OnUpdate();
    }

}