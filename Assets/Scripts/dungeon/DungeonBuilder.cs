using UnityEngine;
using System;
using System.Collections.Generic;
/*
  Class that will create dungeons from configuration and randomly.
 */

public class DungeonBuilder {

    public static int NORTH_DOOR_MASK = 1 << 1;
    public static int EAST_DOOR_MASK = 1 << 2;
    public static int SOUTH_DOOR_MASK = 1 << 3;
    public static int WEST_DOOR_MASK = 1 << 4;

    // Size of the dungeon
    public static int COLS = 10;
    public static int ROWS = 10;

    public class DungeonBuilderOutput {
        public GameObject[] rooms;
        public int entranceIndex;
    }

    public class RoomTemplate {
        /*
          Will be compared to door masks in order to determine whether
          or not the room has doors on the north, south, east and west location.
         */
        public int doorsMask;

        // entrance, exit, ...
        public RoomType roomType;

        // visual appearance
        public InteriorType interiorType;

        // How many enemies.
        public List<EnemyType> enemies = new List<EnemyType>();
    }

    // Load empty rooms from the prefab
    private AssetFactory factory;
    // Parent of the objects that are going to be created.
    private Transform parent;

    public DungeonBuilder(AssetFactory factory, Transform parent) {
        this.factory = factory;
        this.parent = parent;
    }

    /*
      Will generate a dungeon randomly
     */
    public DungeonBuilderOutput GenerateDungeon() {
        RoomTemplate template = new RoomTemplate();
        template.doorsMask = 8;
        template.interiorType = InteriorType.INTERIOR_1;
        template.roomType = RoomType.ENTRANCE;
        template.enemies.Add(EnemyType.ENEMY_1);
        GameObject[] rooms = new GameObject[COLS*ROWS];

        RoomTemplate template2 = new RoomTemplate();
        template2.doorsMask = 2;
        template2.interiorType = InteriorType.INTERIOR_1;
        template2.roomType = RoomType.NORMAL;
        template2.enemies.Add(EnemyType.ENEMY_2);
        template2.enemies.Add(EnemyType.ENEMY_2);

        // For fun. Two rooms.
        GameObject room = CreateRoomFromTemplate(template);
        rooms[GetRoomIndex(0, 0)] = room;
        GameObject room2 = CreateRoomFromTemplate(template2);
        rooms[GetRoomIndex(1, 0)] = room2;

        DungeonBuilderOutput output = new DungeonBuilderOutput();
        output.rooms = rooms;
        output.entranceIndex = GetRoomIndex(0,0);
        return output;
    }

    private int GetRoomIndex(int row, int col) {
        return row * COLS + col;
    }

    /*
      Will create the room from a template and add random enemies
    */
    private GameObject CreateRoomFromTemplate(RoomTemplate template) {
        GameObject room = Instantiate(factory.GetRoomPrefab(template.interiorType), parent);

        // Set the door based on the templates.
        foreach (Transform child in room.transform) {
            Door door = child.GetComponent<Door>();

            // if it's a door.
            if (door != null) {
                if (HasDoor(template.doorsMask, door.DoorId)) {
                    Debug.Log(door.DoorId + " Is active");
                    child.gameObject.SetActive(true);
                } else {
                    child.gameObject.SetActive(false);
                }
            }
        }

        // Add some enemies ;D
        foreach (EnemyType enemyType in template.enemies) {
            Instantiate(factory.GetEnemyPrefab(enemyType), room.transform);
        }

        return room;
    }

    public bool HasDoor(int mask, int doorMask) {
        int afterApply = mask | doorMask;
        return afterApply == mask;
    }

    private GameObject Instantiate(GameObject prefab, Transform theParent) {
        return UnityEngine.Object.Instantiate(prefab, new Vector3(), Quaternion.identity, theParent)
            as GameObject;
    }
}
