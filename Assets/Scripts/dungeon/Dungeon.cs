using UnityEngine;
using System.Collections.Generic;

/*
  A dungeon is a set of rooms that the player explore. It
  is generated programmatically at the beginning of the floor.

  A dungeon has a layout of rooms. The rooms have all the same size
  and no corridor between them so the layout can just be represented
  as a 2D array of integer...

 */
public class Dungeon: MonoBehaviour {

    private AssetFactory factory;
    private DungeonBuilder builder;

    private int currentIndex;

    // Need the player to teleport him correctly.
    private Transform player;

    List<GameObject> rooms = new List<GameObject>();

    public void Start() {
        // We don't care about checking for null. If no player, the game
        // won't work anyway.
        player = FetchUtils.FetchGameObjectByTag(Tags.PLAYER).transform;

        factory = new AssetFactory();
        factory.LoadAll();

        builder = new DungeonBuilder(factory, transform);

        // Now create the first dungeon
        GenerateDungeon();
    }

    private void GenerateDungeon() {
        DungeonBuilder.DungeonBuilderOutput generatedRooms = builder.GenerateDungeon();

        rooms.Clear();

        for (int i = 0; i < generatedRooms.rooms.Length; i++) {

            GameObject room = generatedRooms.rooms[i];
            if (room != null) {

                Debug.Log("ROOM INDEX");
                Debug.Log(i);
                Debug.Log(generatedRooms.entranceIndex);

                if (i != generatedRooms.entranceIndex) {
                    room.SetActive(false);
                }

                // Need to get the room event when a player touch it.
                // for teleporting to the next room.
                foreach (Transform child in room.transform) {
                    Door door = child.GetComponent<Door>();
                    if (door != null) {
                        door.OnEnterDoor += OnEnterDoor;
                    }
                }

                rooms.Add(room);
            } else {
                // We add null to simulate empty room. That way we can easily
                // find where the adjacent rooms are by looking at the index.
                rooms.Add(null);
            }
        }
        currentIndex = generatedRooms.entranceIndex;

        InstantiateAsset(factory.GetEnemyPrefab(EnemyType.ENEMY_1),
                                                new Vector2(0, 0));
    }

    public Room GetCurrentRoom() {
        return rooms[currentIndex].GetComponent<Room>();
    }

    private void OnEnterDoor(int doorId) {
        if (doorId == DungeonBuilder.NORTH_DOOR_MASK) {
            Debug.Log("HIT NORTH");
        } else if (doorId == DungeonBuilder.EAST_DOOR_MASK) {
            Debug.Log("HIT EAST");
        } else if (doorId == DungeonBuilder.SOUTH_DOOR_MASK) {
            Debug.Log("HIT SOUTH");
        } else if (doorId == DungeonBuilder.WEST_DOOR_MASK) {
            Debug.Log("HIT WEST");
        } else {
            // TODO Throw something here
        }
    }

    private GameObject InstantiateAsset(GameObject prefab, Vector2 position) {
        Vector3 position3 = (Vector3) position;
        return Instantiate(prefab, position3, Quaternion.identity, transform) as GameObject;
    }
}
