using UnityEngine;

/*
  Will follow a transform and fire if in line of sight.
  takes a transform as the object is mutable and continously updated
  with the newest position.
 */
public class Seek: ControllerBehaviour {

    [SerializeField]
    private Transform target;

    public Transform Target {
        get {return target;}
        set {target = value;}
    }

    private float positionOffset = 0.3f;

    // First moves within 3pixel to the player.
    private float minDistanceToPlayer = 3f;


    public override Control GetControls() {
        Control control = new Control();

        if (target != null) {
            float distance = Vector2.Distance(target.position, transform.position);

            int compareXCoord = GeometryUtils.compareWithOffset(target.position.x,
                                                                transform.position.x,
                                                                positionOffset);
            int compareYCoord = GeometryUtils.compareWithOffset(target.position.y,
                                                                transform.position.y,
                                                                positionOffset);
            if (distance > minDistanceToPlayer) {
                control.horizontalMovement = compareXCoord;
                control.verticalMovement = compareYCoord;
            } else {
                // Just adjust X then.
                control.horizontalMovement = compareXCoord;
            }

            // Face the player.
            control.horizontalOrientation = compareXCoord;
            control.verticalOrientation = compareYCoord;

            // if same line/column, shoot.
            control.shouldFire = compareXCoord == 0 || compareYCoord == 0;
        }

        return control;
    }
}
