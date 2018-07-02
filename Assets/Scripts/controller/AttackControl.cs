using UnityEngine;

/*
  Sent to the controller
*/
public class AttackControl {

    // For attack - attack controls are blended together so
    public bool shouldFire;

    // > 0 means next weapon, < 0 mean previous weapon
    public int selectNextWeapon;

    // where to shoot
    public Vector2 direction;
}
