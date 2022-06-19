using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour,IInteractable,IFightable {

    private DungeonManager dungeon;

    public EnemyUnit baseUnit;
    [HideInInspector] public EnemyUnit unit;

    private Vector3Int targetPosition;
    private Vector3Int nextPosition;

    public float moveSpeed = 5f;

    public int viewDist = 2;
    public int randomTileRange = 4;

    public void Initialize(DungeonManager dm) {

        unit = ScriptableObject.CreateInstance<EnemyUnit>();
        SetUnitValues();

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

    public void Interact(PlayerManager p) {}

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
        return tiles[Random.Range(0,tiles.Count)];

    }

    private void Move(string dir) {

        int x = nextPosition.x;
        int y = nextPosition.y;

        switch(dir) {

            case "Right":
                x = nextPosition.x+1;
                y = nextPosition.y;
                break;

            case "Left":
                x = nextPosition.x-1;
                y = nextPosition.y;
                break;
            
            case "Up":
                x = nextPosition.x;
                y = nextPosition.y+1;
                break;
            
            case "Down":
                x = nextPosition.x;
                y = nextPosition.y-1;
                break;

        }

        nextPosition = new Vector3Int(x,y,0);

    }

    private bool CanMove(string dir) {

        bool canMove = true;

        int x = nextPosition.x;
        int y = nextPosition.y;

        switch(dir) {

            case "Right":
                x = nextPosition.x+1;
                y = nextPosition.y;
                break;

            case "Left":
                x = nextPosition.x-1;
                y = nextPosition.y;
                break;
            
            case "Up":
                x = nextPosition.x;
                y = nextPosition.y+1;
                break;
            
            case "Down":
                x = nextPosition.x;
                y = nextPosition.y-1;
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

    public void SetUnitValues() {

        unit.enemy = this;

        unit.unitName = baseUnit.unitName.Replace("Enemy","");

        unit.maxHealth = baseUnit.maxHealth;
        unit.currentHealth = baseUnit.currentHealth;

        unit.baseAttackStrength = baseUnit.baseAttackStrength;
        unit.currentAttackStrength = baseUnit.currentAttackStrength;

        unit.baseDefenseStrength = baseUnit.baseDefenseStrength;
        unit.currentDefenseStrength = baseUnit.currentDefenseStrength;

        unit.unitPrefab = baseUnit.unitPrefab;

        unit.abilities = baseUnit.abilities;

    }

}