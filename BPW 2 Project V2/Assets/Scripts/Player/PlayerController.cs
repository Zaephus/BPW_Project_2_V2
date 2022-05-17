using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    private DungeonManager dungeon;

    private Vector3Int targetPosition;

    public float moveSpeed = 5f;

    public void Initialize(DungeonManager dm) {
        
        dungeon = dm;

        targetPosition = Vector3Int.RoundToInt(transform.position);

    }

    public void OnUpdate() {

        transform.position = Vector3.MoveTowards(transform.position,targetPosition,moveSpeed*Time.deltaTime);

        if(Vector3.Distance(transform.position,targetPosition) <= 0.05f) {

            //Keyboard or Stick movement
            if(Input.GetButton("Right") && CanMove("Right")) {
                Move("Right");
            }
            if(Input.GetButton("Left") && CanMove("Left")) {
                Move("Left");
            }
            if(Input.GetButton("Up") && CanMove("Up")) {
                Move("Up");
            }
            if(Input.GetButton("Down") && CanMove("Down")) {
                Move("Down");
            }

            //D-Pad movement
            // if(Input.GetButton("Right") && CanMove("Right")) {
            //     Move("Right");
            // }
            // else if(Input.GetButton("Left") && CanMove("Left")) {
            //     Move("Left");
            // }
            // else if(Input.GetButton("Up") && CanMove("Up")) {
            //     Move("Up");
            // }
            // else if(Input.GetButton("Down") && CanMove("Down")) {
            //     Move("Down");
            // }

        }

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