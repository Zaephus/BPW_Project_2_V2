using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour {

    private DungeonGenerator dungeonGen;

    [SerializeField] private PlayerManager player;

    public Dictionary<Vector3Int,TileType> dungeon = new Dictionary<Vector3Int,TileType>();

    public void Start() {

        dungeonGen = FindObjectOfType<DungeonGenerator>();
        dungeonGen.Generate();

        dungeon = dungeonGen.dungeon;

        player = FindObjectOfType<PlayerManager>();
        player.OnStart();

    }

    public void Update() {
        player.OnUpdate();
    }

    public bool EntityOnTile(Vector3Int targetTile) {
        
        bool entityOnTile = false;

        // foreach(EnemyController enemy in enemies) {
        //     if(enemy.transform.position == targetTile) {
        //         entityOnTile = true;
        //     }
        //     else {
        //         entityOnTile = false;
        //     }
        // }

        if(player.transform.position == targetTile) {
            entityOnTile = true;
        }
        else {
            entityOnTile = false;
        }

        return entityOnTile;

    }

}