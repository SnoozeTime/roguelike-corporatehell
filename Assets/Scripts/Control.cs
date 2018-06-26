using UnityEngine;

/*
  Sent to the controller
*/
public class Control {
    public int horizontalMovement;
    public int verticalMovement;
    public int horizontalOrientation;
    public int verticalOrientation;
    public bool shouldFire;

    // > 0 means next weapon, < 0 mean previous weapon
    public int selectNextWeapon;
}
