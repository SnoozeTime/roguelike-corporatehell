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

    // Level number. Used by the builder to generate the level.
    private int levelNumber = 1;

    private AssetFactory factory;
    private DungeonBuilder builder;

    private int currentIndex;

    // Need the player to teleport him correctly.
    private RogueController player;

    private static float roomWidth = 16.0f;
    private static float roomHeight = 8.0f;

    public static float HalfWidth {
        get { return roomWidth/2;}
    }

    public static float HalfHeight {
        get { return roomHeight/2;}
    }

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
        DungeonBuilder.DungeonBuilderOutput generatedRooms = builder.GenerateDungeon(levelNumber);

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

    // ---------------------------------------------------------
    // Used by the controller to restrict movement near the doors
    // ---------------------------------------------------------

    // A player/controller can go north if the door is opened and he is
    // aligned with it. Also, current obstacles in the room will prevent
    // from moving in a direction.
    public bool CanGoNorth(Vector2 position, float increment) {
        Room room = GetCurrentRoom();
        bool isAligned = (position.x > -.5 && position.x < 0.5);
        bool isInRoom = position.y < 3.5;
        bool somethingInWay = IsSomethingInTheWay(position,
                                                  new Vector2(0, 1),
                                                  increment);
        return !somethingInWay && (isInRoom || (isAligned && room.IsNorthDoorOpened()));
    }

    public bool CanGoSouth(Vector2 position, float increment) {
        Room room = GetCurrentRoom();
        bool isInRoom = position.y > -3.5;
        bool isAligned = (position.x > -.5 && position.x < 0.5);
        bool somethingInWay = IsSomethingInTheWay(position,
                                                  new Vector2(0, -1),
                                                  increment);
        return !somethingInWay && (isInRoom || (isAligned && room.IsSouthDoorOpened()));
    }

    public bool CanGoEast(Vector2 position, float increment) {
        Room room = GetCurrentRoom();
        bool isAligned = (position.y > 0 && position.y < 0.5);
        bool isInRoom = position.x < 7.5;
        bool somethingInWay = IsSomethingInTheWay(position,
                                                  new Vector2(1, 0),
                                                  increment);
        return !somethingInWay && (isInRoom || (isAligned && room.IsEastDoorOpened()));
    }

    public bool CanGoWest(Vector2 position, float increment) {
        Room room = GetCurrentRoom();
        bool isAligned = (position.y > 0 && position.y < 0.5);
        bool isInRoom = position.x > -7.5;
        bool somethingInWay = IsSomethingInTheWay(position,
                                                  new Vector2(-1, 0),
                                                  increment);

        return !somethingInWay && (isInRoom || (isAligned && room.IsWestDoorOpened()));
    }

    // something in the wayyyy! huuum uuuummm... something in the waaaay
    public bool IsSomethingInTheWay(Vector2 currentPosition,
                                    Vector2 direction,
                                    float positionIncrement) {
        Vector2 futurePosition = currentPosition + positionIncrement * direction;
        Room room = GetCurrentRoom();

        foreach (Transform obstacle in room.Obstacles) {
            Collider2D obsCollider = obstacle.gameObject.GetComponent<Collider2D>();

            if (obsCollider == null) {
                Debug.Log("No collider for the obstacle. Is that allowed?");
            } else {
                bool overlap = obsCollider.OverlapPoint(futurePosition);

                if (overlap == true) {
                    return true;
                }
            }

        }

        return false;
    }
}
