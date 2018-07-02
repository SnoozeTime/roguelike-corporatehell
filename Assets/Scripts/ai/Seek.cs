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

    private float positionOffset = 0.5f;

    // First moves within 3pixel to the player.
    private float minDistanceToPlayer = 3f;


    public override Control GetControls() {
        Control control = new Control();
        control.movementControl = new MovementControl();
        control.attackControl = new AttackControl();
        if (target != null) {
            float distance = Vector2.Distance(target.position, transform.position);

            int compareXCoord = GeometryUtils.compareWithOffset(target.position.x,
                                                                transform.position.x,
                                                                positionOffset);
            int compareYCoord = GeometryUtils.compareWithOffset(target.position.y,
                                                                transform.position.y,
                                                                positionOffset);
            if (distance > minDistanceToPlayer) {
                control.movementControl.movement = new Vector2(compareXCoord, compareYCoord);
                            } else {
                // Just adjust X then.
                control.movementControl.movement = new Vector2(compareXCoord, 0);
            }

            // Face the player.
            control.movementControl.orientation = new Vector2(compareXCoord, compareYCoord);

            // if same line/column, shoot.
            control.attackControl.shouldFire = compareXCoord == 0 || compareYCoord == 0;
            control.attackControl.direction = new Vector2(compareXCoord, compareYCoord);
        }

        return control;
    }
}
