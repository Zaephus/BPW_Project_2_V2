using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour {

    private DungeonGenerator dungeonGen;

    [HideInInspector] public PlayerManager player;
    private List<EnemyController> enemies;

    public Dictionary<Vector3Int,TileType> dungeon = new Dictionary<Vector3Int,TileType>();

    public void Start() {

        dungeonGen = FindObjectOfType<DungeonGenerator>();
        dungeonGen.Generate();

        dungeon = dungeonGen.dungeon;

        player = FindObjectOfType<PlayerManager>();
        player.OnStart();

        enemies = new List<EnemyController>(FindObjectsOfType<EnemyController>());

        for(int i = 0; i < enemies.Count; i++) {
            enemies[i].Initialize(this);
        }

    }

    public void Update() {

        player.OnUpdate();

        for(int i = 0; i < enemies.Count; i++) {
            enemies[i].OnUpdate();
        }

    }

    public bool EntityOnTile(Vector3Int targetTile) {
        
        bool entityOnTile = false;

        foreach(EnemyController enemy in enemies) {
            if(Vector3Int.FloorToInt(enemy.transform.position) == targetTile) {
                entityOnTile = true;
            }
            else {
                entityOnTile = false;
            }
        }

        if(player.transform.position == targetTile) {
            entityOnTile = true;
        }
        else {
            entityOnTile = false;
        }

        return entityOnTile;

    }

}