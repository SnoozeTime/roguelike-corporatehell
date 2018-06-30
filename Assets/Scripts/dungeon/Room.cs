using UnityEngine;
using System.Collections.Generic;

/*
  Represent a room :D
  Need this because I had code duplication when checking the existance of doors.
*/
public class Room: MonoBehaviour {

    private List<Door> doors;

    // True if all enemies have been killed
    private bool IsCompleted;

    // Will open the door if enemies alive < 0
    private int enemiesAlive;

    public void Start() {
        doors = FetchUtils.FetchChildrenWithComponent<Door>(transform);
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
            if (afterApply == door.DoorId && door.gameObject.activeSelf) {
                return true;
            }
        }

        return false;
    }

    private bool IsDoorOpened(int doorMask) {
        Door door = GetDoor(doorMask);

        if (door != null && door.gameObject.activeSelf) {
            return door.IsOpened;
        }

        return false;
    }

    // Will only return active doors...
    private Door GetDoor(int doorMask) {
        foreach (Door door in doors) {
            int afterApply = door.DoorId | doorMask;
            if (afterApply == door.DoorId && door.gameObject.activeSelf) {
                return door;
            }
        }

        return null;
    }

    public void OnEnemyKilled(GameObject go) {
        enemiesAlive -= 1;

        if (enemiesAlive <= 0) {
            foreach (Door door in doors) {
                door.Open();
            }
        }
    }
}
