using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    private DungeonManager dungeon;

    private PlayerController controller;

    public void OnStart() {

        dungeon = FindObjectOfType<DungeonManager>();

        controller = GetComponent<PlayerController>();
        controller.Initialize(dungeon);

    }

    public void OnUpdate() {
        controller.OnUpdate();
    }

}