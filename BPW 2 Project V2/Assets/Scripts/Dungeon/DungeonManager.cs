using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour {

    private DungeonGenerator dungeonGen;

    [HideInInspector] public PlayerManager player;
    public List<EnemyController> enemies;

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

    public IEnumerator ResetDungeon() {

        for(int i = 0; i < dungeonGen.wallTransform.childCount; i++) {
            Destroy(dungeonGen.wallTransform.GetChild(i).gameObject);
        }
        for(int i = 0; i < dungeonGen.floorTransform.childCount; i++) {
            Destroy(dungeonGen.floorTransform.GetChild(i).gameObject);
        }
        for(int i = 0; i < dungeonGen.entityTransform.childCount; i++) {
            Destroy(dungeonGen.entityTransform.GetChild(i).gameObject);
        }
        
        Destroy(player);

        for(int i = 0; i < enemies.Count; i++) {
            Destroy(enemies[i]);
        }

        yield return new WaitForEndOfFrame();

        dungeon.Clear();
        enemies.Clear();

        dungeonGen.Generate();

        player = FindObjectOfType<PlayerManager>();
        player.OnStart();

        enemies = new List<EnemyController>(FindObjectsOfType<EnemyController>());

        for(int i = 0; i < enemies.Count; i++) {
            enemies[i].Initialize(this);
        }

    }

    public void Update() {

        for(int i = 0; i < enemies.Count; i++) {
            enemies[i]?.OnUpdate();
        }
        
        player?.OnUpdate();

    }

    public bool EntityOnTile(Vector3Int targetTile) {
        
        bool entityOnTile = false;

        for(int i = 0; i < enemies.Count; i++) {
            if(Vector3Int.FloorToInt(enemies[i].transform.position) == targetTile) {
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