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
        control.shouldFire = (int) Input.GetAxisRaw(InputConstants.FIRE) > 0 ? true : false;
        return control;
    }
}
