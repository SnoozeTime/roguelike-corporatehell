using UnityEngine;

public class PlayerBehaviour: ControllerBehaviour {

    public override Control GetControls() {
        Control control = new Control();
        control.horizontalMovement = (int) Input.GetAxisRaw(InputConstants.HORIZONTAL);
        control.verticalMovement = (int) Input.GetAxisRaw(InputConstants.VERTICAL);
        // If face buttons are pressed, we are going to use them for orientation
        // instead of just walking.
        control.horizontalOrientation = (int) Input.GetAxisRaw(InputConstants.FACE_HORIZONTAL);
        control.verticalOrientation = (int) Input.GetAxisRaw(InputConstants.FACE_VERTICAL);
        control.shouldFire = control.horizontalOrientation != 0 || control.verticalOrientation != 0;
        return control;
    }
}
