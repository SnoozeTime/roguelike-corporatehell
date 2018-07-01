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
    public static int COLS = 20;
    public static int ROWS = 20;

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

    // How to generate the map
    private GenerationAlgorithm algo;

    public DungeonBuilder(AssetFactory factory, Transform parent) {
        this.factory = factory;
        this.parent = parent;
        algo = new NaiveAlgorithm();
    }

    private void DebugPrintLayout(List<List<RoomType>> layout) {
        string debugString = "";
        for (int row = 0; row < layout.Count; row++) {

            for (int col = 0; col < layout[row].Count; col++) {
                debugString += layout[row][col].ToString();
                debugString += ",";
            }

            debugString += "\n";
        }
        Debug.Log(debugString);
    }

    /*
      Will generate a dungeon randomly
     */
    public DungeonBuilderOutput GenerateDungeon(int levelNumber) {

        List<List<RoomType>> layout = algo.GenerateMapLayout();
        DebugPrintLayout(layout);
        GameObject[] rooms = new GameObject[COLS*ROWS];
        DungeonBuilderOutput output = new DungeonBuilderOutput();

        for (int row = 0; row < layout.Count; row++) {
            for (int col = 0; col < layout[row].Count; col++) {

                if (layout[row][col] != RoomType.NONE) {

                    int roomIndex = GetRoomIndex(row, col);
                    int doorMask = ComputeDoorMask(col, row, layout);
                    RoomTemplate template = CreateRandomTemplate(layout[row][col], doorMask);
                    rooms[roomIndex] = CreateRoomFromTemplate(template);

                    if (layout[row][col] == RoomType.ENTRANCE) {
                        output.entranceIndex = roomIndex;
                    }
                }
            }
        }
        output.rooms = rooms;
        return output;
    }

    private int GetRoomIndex(int row, int col) {
        return row * COLS + col;
    }

    /*
      Each room type (entrance, normal, ...) will have some template that
      can be chosen from. Then, there is some randomness (e.g. number of enemies)
     */
    private RoomTemplate CreateRandomTemplate(RoomType roomType, int doorMask) {
        RoomTemplate template = new RoomTemplate();
        template.doorsMask = doorMask;
        template.interiorType = InteriorType.INTERIOR_1;
        template.roomType = roomType;
        template.enemies.Add(EnemyType.ENEMY_2);
        return template;
    }

    private int ComputeDoorMask(int colIndex, int rowIndex, List<List<RoomType>> layout) {
        int doorMask = 0;
        if (colIndex > 0) {
            if (layout[rowIndex][colIndex-1] != RoomType.NONE) {
                doorMask |= WEST_DOOR_MASK;
            }
        }

        if (colIndex < COLS - 1) {
            if (layout[rowIndex][colIndex+1] != RoomType.NONE) {
                doorMask |= EAST_DOOR_MASK;
            }
        }

        if (rowIndex > 0) {
            if (layout[rowIndex-1][colIndex] != RoomType.NONE) {
                doorMask |= NORTH_DOOR_MASK;
            }
        }

        if (rowIndex < ROWS - 1) {
            if (layout[rowIndex+1][colIndex] != RoomType.NONE) {
                doorMask |= SOUTH_DOOR_MASK;
            }
        }

        return doorMask;
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
                    child.gameObject.SetActive(true);
                } else {
                    child.gameObject.SetActive(false);
                }
            }
        }

        // Add some enemies ;D
        foreach (EnemyType enemyType in template.enemies) {
            GameObject enemy = Instantiate(factory.GetEnemyPrefab(enemyType), room.transform);
            enemy.GetComponent<Health>().OnNoHp += room.GetComponent<Room>().OnEnemyKilled;
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
