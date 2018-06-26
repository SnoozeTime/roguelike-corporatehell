using UnityEngine;

public class PlayerBehaviour: ControllerBehaviour {

    // To simulate pressed down/up
    private bool selectWeaponPressed = false;

    public override Control GetControls() {
        Control control = new Control();
        control.horizontalMovement = (int) Input.GetAxisRaw(InputConstants.HORIZONTAL);
        control.verticalMovement = (int) Input.GetAxisRaw(InputConstants.VERTICAL);
        // If face buttons are pressed, we are going to use them for orientation
        // instead of just walking.
        control.horizontalOrientation = (int) Input.GetAxisRaw(InputConstants.FACE_HORIZONTAL);
        control.verticalOrientation = (int) Input.GetAxisRaw(InputConstants.FACE_VERTICAL);
        control.shouldFire = control.horizontalOrientation != 0 || control.verticalOrientation != 0;


        int weaponPressedInput = (int) Input.GetAxisRaw(InputConstants.NEXT_WEAPON);
        if (weaponPressedInput == 0) {
            selectWeaponPressed = false;
        } else {
            if (!selectWeaponPressed) {
                control.selectNextWeapon = (int) Input.GetAxisRaw(InputConstants.NEXT_WEAPON);
                selectWeaponPressed = true;
            }
        }

        return control;
    }
}
