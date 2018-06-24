using UnityEngine;
using System.Collections.Generic;

/*
  Represent a room :D
  Need this because I had code duplication when checking the existance of doors.
*/
public class Room: MonoBehaviour {

    private List<Door> doors;

    public void Start() {

        doors = new List<Door>();
        // Populate the doors by looking at its children objects.
        foreach (Transform child in transform) {
            Door door = child.GetComponent<Door>();

            // if it's a door.
            if (door != null) {
                doors.Add(door);
            }
        }

    }

    public List<Door> Doors {
        get {return doors;}
    }

    // -------------------------------------
    // Only 4 doors, so I duplicate a bit the code to avoid
    // having to put the door masks everywhere.
    public bool HasNorthDoor() {
        return HasDoor(DungeonBuilder.NORTH_DOOR_MASK);
    }

    public bool HasEastDoor() {
        return HasDoor(DungeonBuilder.EAST_DOOR_MASK);
    }

    public bool HasSouthDoor() {
        return HasDoor(DungeonBuilder.SOUTH_DOOR_MASK);
    }

    public bool HasWestDoor() {
        return HasDoor(DungeonBuilder.WEST_DOOR_MASK);
    }

    public bool IsNorthDoorOpened() {
        return IsDoorOpened(DungeonBuilder.NORTH_DOOR_MASK);
    }

    public bool IsEastDoorOpened() {
        return IsDoorOpened(DungeonBuilder.EAST_DOOR_MASK);
    }

    public bool IsSouthDoorOpened() {
        return IsDoorOpened(DungeonBuilder.SOUTH_DOOR_MASK);
    }

    public bool IsWestDoorOpened() {
        return IsDoorOpened(DungeonBuilder.WEST_DOOR_MASK);
    }

    // -------------------------------------
    private bool HasDoor(int doorMask) {
        foreach (Door door in doors) {
            int afterApply = door.DoorId | doorMask;
            if (afterApply == door.DoorId) {
                return true;
            }
        }

        return false;
    }

    private bool IsDoorOpened(int doorMask) {
        Door door = GetDoor(doorMask);

        if (door != null) {
            return door.IsOpened;
        }

        return false;
    }

    private Door GetDoor(int doorMask) {
        foreach (Door door in doors) {
            int afterApply = door.DoorId | doorMask;
            if (afterApply == door.DoorId) {
                return door;
            }
        }

        return null;
    }
}
