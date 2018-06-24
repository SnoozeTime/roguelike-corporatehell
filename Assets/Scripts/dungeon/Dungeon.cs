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
    private RogueController player;

    List<GameObject> rooms = new List<GameObject>();

    public void Awake() {
        // We don't care about checking for null. If no player, the game
        // won't work anyway.
        player = FetchUtils.FetchGameObjectByTag(Tags.PLAYER).GetComponent<RogueController>();

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
    }

    public Room GetCurrentRoom() {
        return rooms[currentIndex].GetComponent<Room>();
    }

    private void OnEnterDoor(int doorId) {
        // first, change the player position.
        TeleportPlayer(doorId);

        int nextIndex = GetNextRoomIndex(doorId);
        ChangeRoom(currentIndex, nextIndex);
    }

    private int GetNextRoomIndex(int doorId) {
        if (doorId == DungeonBuilder.NORTH_DOOR_MASK) {
            return currentIndex - DungeonBuilder.COLS;
        } else if (doorId == DungeonBuilder.EAST_DOOR_MASK) {
            return currentIndex + 1;
        } else if (doorId == DungeonBuilder.SOUTH_DOOR_MASK) {
            return currentIndex + DungeonBuilder.COLS;
        } else if (doorId == DungeonBuilder.WEST_DOOR_MASK) {
            return currentIndex - 1;
        } else {
            // TODO Throw something here
            throw new System.ArgumentException("Door Id should be a valid value");
        }

    }
    private void TeleportPlayer(int doorId) {
        Vector2 newOrientation;
        Vector2 newPosition;
        if (doorId == DungeonBuilder.NORTH_DOOR_MASK) {
            newPosition = new Vector2(0f, -3.4f);
            newOrientation = new Vector2(0f, -1f);
        } else if (doorId == DungeonBuilder.EAST_DOOR_MASK) {
            newPosition = new Vector2(-7.4f, 0f);
            newOrientation = new Vector2(1f, 0f);
        } else if (doorId == DungeonBuilder.SOUTH_DOOR_MASK) {
            newPosition = new Vector2(0f, 3.4f);
            newOrientation = new Vector2(0f, 1f);
        } else if (doorId == DungeonBuilder.WEST_DOOR_MASK) {
            newPosition = new Vector2(7.4f, 0f);
            newOrientation = new Vector2(-1f, 0f);
        } else {
            // TODO Throw something here
            throw new System.ArgumentException("Door Id should be a valid value");
        }

        player.transform.position = newPosition;
        player.Orientation = newOrientation;
    }

    private void ChangeRoom(int previousIndex, int nextIndex) {
        // Enable next room and disable previous room
        rooms[previousIndex].SetActive(false);
        rooms[nextIndex].SetActive(true);

        currentIndex = nextIndex;
    }

    private GameObject InstantiateAsset(GameObject prefab, Vector2 position) {
        Vector3 position3 = (Vector3) position;
        return Instantiate(prefab, position3, Quaternion.identity, transform) as GameObject;
    }

    // ---------------------------------------------------------
    // Used by the controller to restrict movement near the doors
    // ---------------------------------------------------------

    // A player/controller can go north if the door is opened and he is
    // aligned with it.
    public bool CanGoNorth(Vector2 position) {
        Room room = GetCurrentRoom();
        bool isAligned = (position.x > -.5 && position.x < 0.5);
        bool isInRoom = position.y < 3.5;

        return isInRoom || (isAligned && room.IsNorthDoorOpened());
    }

    public bool CanGoSouth(Vector2 position) {
        Room room = GetCurrentRoom();
        bool isInRoom = position.y > -3.5;
        bool isAligned = (position.x > -.5 && position.x < 0.5);

        return isInRoom || (isAligned && room.IsSouthDoorOpened());
    }

    public bool CanGoEast(Vector2 position) {
        Room room = GetCurrentRoom();
        bool isAligned = (position.y > 0 && position.y < 0.5);
        bool isInRoom = position.x < 7.5;
        return isInRoom || (isAligned && room.IsEastDoorOpened());
    }

    public bool CanGoWest(Vector2 position) {
        Room room = GetCurrentRoom();
        bool isAligned = (position.y > 0 && position.y < 0.5);
        bool isInRoom = position.x > -7.5;

        return isInRoom || (isAligned && room.IsWestDoorOpened());
    }
}
