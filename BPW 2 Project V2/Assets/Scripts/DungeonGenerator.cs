using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileType {Room,Corridor,Wall}
public enum EntityType {Player,GhostEnemy}

public class DungeonGenerator : MonoBehaviour {

    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public int seed = 0;

    public int gridWidth = 30;
    public int gridHeight = 30;

    public int enemyAmount = 8;

    public int spaceBetweenRooms = 3;

    public int numRooms = 6;
    public int minRoomSize = 4;
    public int maxRoomSize = 7;
    public List<Room> roomList = new List<Room>();

    public Dictionary<Vector3Int,TileType> dungeon = new Dictionary<Vector3Int,TileType>();
    public Dictionary<Vector3Int,EntityType> entities = new Dictionary<Vector3Int,EntityType>();

    public void Generate() {

        GetSeed();
        AllocateRooms();
        ConnectRooms();
        AllocateWalls();
        AllocatePlayer();
        AllocateEnemies();
        SpawnDungeon();

        //SaveSystem.instance.SaveSeed(seed,"DungeonSeed");

    }

    public void GetSeed() {

        //seed = SaveSystem.instance.LoadSeed("DungeonSeed");

        if(seed == 0) {
            seed = Random.Range(1000,9999);
        }
        Random.InitState(seed);

    }

    public void AllocateRooms() {

        int safety = 0;

        for(int i = 0; i < numRooms; i++) {

            safety++;

            int minX = Random.Range(0,gridWidth);
            int maxX = minX + Random.Range(minRoomSize,maxRoomSize+1);

            int minY = Random.Range(0,gridHeight);
            int maxY = minY + Random.Range(minRoomSize,maxRoomSize+1);

            Room room = new Room(minX,maxX,minY,maxY);

            if(CanRoomFit(room)) {
                AddRoom(room);
            }
            else if(safety <= (numRooms*100)) {
                i--;
            }

        }

    }

    public void AddRoom(Room room) {
        for(int x = room.minX; x <= room.maxX; x++) {
            for(int y = room.minY; y <= room.maxY; y++) {
                dungeon.Add(new Vector3Int(x,y,0),TileType.Room);
            }
        }
        roomList.Add(room);
    }

    public bool CanRoomFit(Room room) {
        for(int x = room.minX-spaceBetweenRooms; x <= room.maxX+spaceBetweenRooms; x++) {
            for(int y = room.minY-spaceBetweenRooms; y <= room.maxY+spaceBetweenRooms; y++) {
                if(dungeon.ContainsKey(new Vector3Int(x,y,0))) {
                    return false;
                }
            }
        }
        return true;
    }

    public void ConnectRooms() {

        for(int i = 0; i < roomList.Count; i++) {

            Room roomOne = roomList[i];

            for(int j = 0; j < roomList.Count; j++) {

                if(j == i) {
                    continue;
                }

                Room roomTwo = roomList[j];

                if(CanConnectRooms(roomOne,roomTwo)) {
                    if((roomTwo.GetCenter().x < roomOne.minX && roomTwo.GetCenter().x > roomOne.maxX) || (roomTwo.GetCenter().y < roomOne.minY && roomTwo.GetCenter().y > roomOne.maxY)) {
                        if(!roomOne.connectedRooms.Contains(roomTwo) && !roomTwo.connectedRooms.Contains(roomOne)) {
                            AllocateCorridors(roomOne,roomTwo);
                        }
                    }
                    else {
                        AllocateCorridors(roomOne,roomTwo);
                    }
                }

            }

        }

        for(int i = 0; i < roomList.Count; i++) {

            Room room = roomList[i];

            if(room.connectedRooms.Count == 0) {

                roomList.Remove(room);

                for(int x = room.minX; x <= room.maxX; x++) {
                    for(int y = room.minY; y <= room.maxY; y++) {
                        dungeon.Remove(new Vector3Int(x,y,0));
                    }
                }
            }
        }

    }
    
    public void AllocateCorridors(Room roomOne,Room roomTwo) {

        Vector3Int posOne = roomOne.GetCenter();
        Vector3Int posTwo = roomTwo.GetCenter();

        roomOne.connectedRooms.Add(roomTwo);
        roomTwo.connectedRooms.Add(roomOne);

        int dirX = posTwo.x > posOne.x ? 1 : -1;
        int x = 0;
        for(x = posOne.x; x != posTwo.x; x += dirX) {
            Vector3Int pos = new Vector3Int(x,posOne.y,0);
            if(dungeon.ContainsKey(pos)) {
                continue;
            }
            dungeon.Add(pos,TileType.Corridor);
        }

        int dirY = posTwo.y > posOne.y ? 1 : -1;
        for(int y = posOne.y; y != posTwo.y; y += dirY) {
            Vector3Int pos = new Vector3Int(x,y,0);
            if(dungeon.ContainsKey(pos)) {
                continue;
            }
            dungeon.Add(pos,TileType.Corridor);
        }

    }

    public bool CanConnectRooms(Room roomOne,Room roomTwo) {

        Vector3Int posOne = roomOne.GetCenter();
        Vector3Int posTwo = roomTwo.GetCenter();

        int corridorBuffer = 3;

        int dirX = posTwo.x > posOne.x ? 1 : -1;
        int x = 0;
        for(x = posOne.x; x != posTwo.x; x += dirX) {
            Vector3Int pos = new Vector3Int(x,posOne.y,0);
            if(dungeon.ContainsKey(pos)) {
                if(dungeon[pos] == TileType.Room) {
                    if(!roomOne.IsPointInRoom(pos) && !roomTwo.IsPointInRoom(pos)) {
                        return false;
                    }
                }
            }
            for(int i = -corridorBuffer; i <= corridorBuffer; i++) {
                TileType value;
                if(dungeon.TryGetValue(new Vector3Int(x,posOne.y+i,0),out value)) {
                    if(value == TileType.Corridor) {
                        return false;
                    }
                }
            }
        }

        int dirY = posTwo.y > posOne.y ? 1 : -1;
        for(int y = posOne.y; y != posTwo.y; y += dirY) {
            Vector3Int pos = new Vector3Int(x,y,0);
            if(dungeon.ContainsKey(pos)) {
                if(dungeon[pos] == TileType.Room) {
                    if(!roomOne.IsPointInRoom(pos) && !roomTwo.IsPointInRoom(pos)) {
                        return false;
                    }
                }
            }
            for(int i = -corridorBuffer; i <= corridorBuffer; i++) {
                TileType value;
                if(dungeon.TryGetValue(new Vector3Int(x+i,y,0),out value)) {
                    if(value == TileType.Corridor) {
                        return false;
                    }
                }
            }
        }

        return true;

    }

    public void AllocateWalls() {

        List<Vector3Int> keys = dungeon.Keys.ToList();

        foreach(Vector3Int kv in keys) {
            for(int x = -1; x <= 1; x++) {
                for(int y = -1; y <= 1; y++) {
                    Vector3Int pos = kv + new Vector3Int(x,y,0);
                    if(dungeon.ContainsKey(pos)) {
                        continue;
                    }
                    dungeon.Add(pos,TileType.Wall);
                }
            }
        }

    }

    public void AllocatePlayer() {

        int randomRoomNumber = Random.Range(0,roomList.Count);
        Room randomRoom = roomList[randomRoomNumber];
        roomList[randomRoomNumber].containsPlayer = true;
        entities.Add(randomRoom.GetRandomTile(),EntityType.Player);

    }

    public void AllocateEnemies() {

        List<Room> tempRooms = new List<Room>();
        foreach(Room r in roomList) {
            if(!r.containsPlayer) {
                tempRooms.Add(r);
            }
        }

        for(int i = 0; i < enemyAmount; i++) {
            Room randomRoom = tempRooms[Random.Range(0,tempRooms.Count)];
            Vector3Int pos = randomRoom.GetRandomTile();

            if(entities.ContainsKey(pos)) {
                i--;
            }
            else {
                entities.Add(pos,EntityType.GhostEnemy);
            }
        }

    }

    public void SpawnDungeon() {

        foreach(KeyValuePair<Vector3Int,TileType> kv in dungeon) {
            switch(kv.Value) {

                case TileType.Room:
                case TileType.Corridor:
                    Instantiate(floorPrefab,kv.Key,Quaternion.identity,transform);
                    break;

                case TileType.Wall:
                    Instantiate(wallPrefab,kv.Key,Quaternion.identity,transform);
                    break;

            }
        }

        foreach(KeyValuePair<Vector3Int,EntityType> kv in entities) {
            switch(kv.Value) {

                case EntityType.Player:
                    GameObject player = Instantiate(playerPrefab,kv.Key,Quaternion.identity);
                    //player.GetComponent<PlayerController>().startPos.x = kv.Key.x;
                    //player.GetComponent<PlayerController>().startPos.y = kv.Key.y;
                    break;

                case EntityType.GhostEnemy:
                    GameObject enemy = Instantiate(enemyPrefab,kv.Key,Quaternion.identity);
                    //enemy.GetComponent<EnemyController>().startPos.x = kv.Key.x;
                    //enemy.GetComponent<EnemyController>().startPos.y = kv.Key.y;
                    break;

            }
        }

    }

}

public class Room {

    public int minX;
    public int maxX;

    public int minY;
    public int maxY;

    public bool containsPlayer;

    public List<Room> connectedRooms = new List<Room>();

    public Room(int _minX,int _maxX,int _minY,int _maxY) {
        minX = _minX;
        maxX = _maxX;
        minY = _minY;
        maxY = _maxY;
    }

    public Vector3Int GetCenter() {
        return new Vector3Int(Mathf.RoundToInt(Mathf.Lerp(minX,maxX,0.5f)),Mathf.RoundToInt(Mathf.Lerp(minY,maxY,0.5f)),0);
    }

    public Vector3Int GetRandomTile() {
        return new Vector3Int(Mathf.RoundToInt(Random.Range(minX,maxX+1)),Mathf.RoundToInt(Random.Range(minY,maxY+1)),0);
    }

    public bool IsPointInRoom(Vector3Int point) {
        return point.x >= minX-1 && point.x <= maxX+1 && point.y >= minY-1 && point.y <= maxY+1;
    }

}