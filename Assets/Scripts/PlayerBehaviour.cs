using UnityEngine;

public class PlayerBehaviour: ControllerBehaviour {

    // To simulate pressed down/up
    private bool selectWeaponPressed = false;

    public override Control GetControls() {
        Control control = new Control();
        control.movementControl = new MovementControl();
        control.attackControl = new AttackControl();

        int movementX = (int) Input.GetAxisRaw(InputConstants.HORIZONTAL);
        int movementY = (int) Input.GetAxisRaw(InputConstants.VERTICAL);
        int orientationX = (int) Input.GetAxisRaw(InputConstants.FACE_HORIZONTAL);
        int orientationY = (int) Input.GetAxisRaw(InputConstants.FACE_VERTICAL);
        control.movementControl.movement = new Vector2(movementX, movementY);
        control.movementControl.orientation = new Vector2(orientationX, orientationY);

        // The player will fire in the direction he is facing
        control.attackControl.shouldFire = orientationX != 0 || orientationY != 0;
        Vector2 attackDirection = new Vector2();
        if (orientationX != 0) {
            attackDirection.x = orientationX;
            attackDirection.y = 0;
        }

        if (orientationY != 0) {
            attackDirection.x = 0;
            attackDirection.y = orientationY;
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
