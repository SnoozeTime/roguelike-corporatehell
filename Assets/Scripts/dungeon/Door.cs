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

    void OnTriggerEnter2D(Collider2D other) {
        // If player, trigger an event.
    }

    public int DoorId {
        get {return doorId;}
    }
}
