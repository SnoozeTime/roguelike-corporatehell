using UnityEngine;

public class PlayerBehaviour: ControllerBehaviour {

    // To simulate pressed down/up
    private bool selectWeaponPressed = false;

    public override Control GetControls() {
        Control control = new Control();
        control.movementControl = new MovementControl();
        control.attackControl = new AttackControl();

        control.movementControl.horizontalMovement = (int) Input.GetAxisRaw(InputConstants.HORIZONTAL);
        control.movementControl.verticalMovement = (int) Input.GetAxisRaw(InputConstants.VERTICAL);
        // If face buttons are pressed, we are going to use them for orientation
        // instead of just walking.
        control.movementControl.horizontalOrientation = (int) Input.GetAxisRaw(InputConstants.FACE_HORIZONTAL);
        control.movementControl.verticalOrientation = (int) Input.GetAxisRaw(InputConstants.FACE_VERTICAL);


        // The player will fire in the direction he is facing
        control.attackControl.shouldFire = control.movementControl.horizontalOrientation != 0 || control.movementControl.verticalOrientation != 0;
        Vector2 attackDirection = new Vector2();
        if (control.movementControl.horizontalOrientation != 0) {
            attackDirection.x = control.movementControl.horizontalOrientation;
            attackDirection.y = 0;
        }

        if (control.movementControl.verticalOrientation != 0) {
            attackDirection.x = 0;
            attackDirection.y = control.movementControl.verticalOrientation;
        }
        control.attackControl.direction = attackDirection;

        int weaponPressedInput = (int) Input.GetAxisRaw(InputConstants.NEXT_WEAPON);
        if (weaponPressedInput == 0) {
            selectWeaponPressed = false;
        } else {
            if (!selectWeaponPressed) {
                control.attackControl.selectNextWeapon = (int) Input.GetAxisRaw(InputConstants.NEXT_WEAPON);
                selectWeaponPressed = true;
            }
        }

        return control;
    }
}
