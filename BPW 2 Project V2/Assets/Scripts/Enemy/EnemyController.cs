using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private DungeonManager dungeon;

    private Vector3Int targetPosition;
    private Vector3Int nextPosition;

    public float moveSpeed = 5f;

    public int viewDist = 2;
    public int randomTileRange = 4;

    public void Initialize(DungeonManager dm) {

        dungeon = dm;

        nextPosition = Vector3Int.RoundToInt(transform.position);
        targetPosition = Vector3Int.RoundToInt(transform.position);

    }

    public void OnUpdate() {

        if(Vector3.Distance(transform.position,dungeon.player.transform.position) <= viewDist) {
            MoveTowardsTarget(Vector3Int.FloorToInt(dungeon.player.transform.position));
        }
        else {
            MoveTowardsTarget(targetPosition);
        }

        if(Vector3.Distance(transform.position,targetPosition) <= 0.05f || Vector3.Distance(transform.position,targetPosition) >= viewDist) {
            targetPosition = GetTargetTile();
        }

    }

    public void MoveTowardsTarget(Vector3Int target) {

        transform.position = Vector3.MoveTowards(transform.position,nextPosition,moveSpeed*Time.deltaTime);

        if(Vector3.Distance(transform.position,nextPosition) <= 0.05f) {

            if(Mathf.Abs(target.x-transform.position.x) >= Mathf.Abs(target.y-transform.position.y)) {

                if(target.x > transform.position.x) {
                    if(CanMove("Right")) {
                        Move("Right");
                    }
                    else {
                        if(target.y < transform.position.y) {
                            if(CanMove("Down")) {
                                Move("Down");
                            }
                        }
                        else if(target.y > transform.position.y) {
                            if(CanMove("Up")) {
                                Move("Up");
                            }
                        }
                    }
                }
                else if(target.x < transform.position.x) {
                    if(CanMove("Left")) {
                        Move("Left");
                    }
                    else {
                        if(target.y < transform.position.y) {
                            if(CanMove("Down")) {
                                Move("Down");
                            }
                        }
                        else if(target.y > transform.position.y) {
                            if(CanMove("Up")) {
                                Move("Up");
                            }
                        }
                    }
                }
            }

            if(Mathf.Abs(target.x-transform.position.x) < Mathf.Abs(target.y-transform.position.y)) {

                if(target.y < transform.position.y) {
                    if(CanMove("Down")) {
                        Move("Down");
                    }
                    else {
                        if(target.x > transform.position.x) {
                            if(CanMove("Right")) {
                                Move("Right");
                            }
                        }
                        else if(target.x < transform.position.x) {
                            if(CanMove("Left")) {
                                Move("Left");
                            }
                        }
                    }
                }
                else if(target.y > transform.position.y) {
                    if(CanMove("Up")) {
                        Move("Up");
                    }
                    else {
                        if(target.x > transform.position.x) {
                            if(CanMove("Right")) {
                                Move("Right");
                            }
                        }
                        else if(target.x < transform.position.x) {
                            if(CanMove("Left")) {
                                Move("Left");
                            }
                        }
                    }
                }
            }

        }

    }

    public Vector3Int GetTargetTile() {

        List<Vector3Int> tiles = new List<Vector3Int>();

        for(int x = (int)(transform.position.x) - randomTileRange; x <= (int)(transform.position.x) + randomTileRange; x++) {
            for(int y = (int)(transform.position.y) - randomTileRange; y <= (int)(transform.position.y) + randomTileRange; y++) {

                Vector3Int pos = new Vector3Int(x,y,0);
                TileType value;

                if(dungeon.dungeon.TryGetValue(pos,out value)) {
                    if(value != TileType.Wall) {
                        tiles.Add(pos);
                    }
                }

            }
        }
        Debug.Log(tiles[Random.Range(0,tiles.Count)]);
        return tiles[Random.Range(0,tiles.Count)];

    }

    private void Move(string dir) {

        int x = targetPosition.x;
        int y = targetPosition.y;

        switch(dir) {

            case "Right":
                x = targetPosition.x+1;
                y = targetPosition.y;
                break;

            case "Left":
                x = targetPosition.x-1;
                y = targetPosition.y;
                break;
            
            case "Up":
                x = targetPosition.x;
                y = targetPosition.y+1;
                break;
            
            case "Down":
                x = targetPosition.x;
                y = targetPosition.y-1;
                break;

        }

        targetPosition = new Vector3Int(x,y,0);

    }

    private bool CanMove(string dir) {

        bool canMove = true;

        int x = targetPosition.x;
        int y = targetPosition.y;

        switch(dir) {

            case "Right":
                x = targetPosition.x+1;
                y = targetPosition.y;
                break;

            case "Left":
                x = targetPosition.x-1;
                y = targetPosition.y;
                break;
            
            case "Up":
                x = targetPosition.x;
                y = targetPosition.y+1;
                break;
            
            case "Down":
                x = targetPosition.x;
                y = targetPosition.y-1;
                break;

        }

        Vector3Int pos = new Vector3Int(x,y,0);

        if(dungeon.dungeon.ContainsKey(pos)) {
            if(dungeon.dungeon[pos] == TileType.Wall || dungeon.EntityOnTile(pos)) {
                canMove = false;
            }
        }
        else {
            canMove = true;
        }

        return canMove;
        
    }

}