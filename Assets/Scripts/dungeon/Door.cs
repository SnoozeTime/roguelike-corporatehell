using UnityEngine;

/*
  Door has a direction. Can be closed or opened.
  Enabled -> You can go to the door next to it.
 */
public class Door: MonoBehaviour {


    // Defined in dungeon builder (TODO Use enum?)
    [SerializeField]
    private int doorId;

    [SerializeField]
    private bool isOpened;

    // Triggered when player walks on it. Will send
    // the door id
    public delegate void EnterDoor(int doorId);
    public event EnterDoor OnEnterDoor;

    void OnTriggerEnter2D(Collider2D other) {
        // If player, trigger an event
        if (other.gameObject.tag == EnumUtils.StringValueOf(Tags.PLAYER)) {
            if (OnEnterDoor != null) {
                OnEnterDoor(doorId);
            }
        }
    }

    public int DoorId {
        get {return doorId;}
    }

    public bool IsOpened {
        get {return isOpened;}
        set {isOpened = value;}
    }

    public void Open() {
        isOpened = true;
    }
}
