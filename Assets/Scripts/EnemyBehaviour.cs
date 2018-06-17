using UnityEngine;

/*
  Simple behaviour. Follow the player and try to shoot him.
*/
public class EnemyBehaviour: ControllerBehaviour {

    private Transform playerTransform;

    private float positionOffset = 0.3f;

    // First moves within 3pixel to the player.
    private float minDistanceToPlayer = 3f;

    public void Start() {
        // need to follow the player.
        GameObject[] players = GameObject.FindGameObjectsWithTag(
            EnumUtils.StringValueOf(Tags.PLAYER)
            );

        // We don't care about checking the length. if no player, the game
        // won't work anyway.
        playerTransform = players[0].transform;
    }

    public override Control GetControls() {
        Control control = new Control();

        // Try to go to the player, but keep distance :D
        float distance = Vector2.Distance(playerTransform.position, transform.position);

        int compareXCoord = compareWithOffset(playerTransform.position.x,
                                              transform.position.x,
                                              positionOffset);
        int compareYCoord = compareWithOffset(playerTransform.position.y,
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

        return control;
    }

    private int compareWithOffset(float coord, float otherCoord, float offset) {

        if (coord < otherCoord - offset) {
            return -1;
        } else if (coord > otherCoord + offset) {
            return 1;
        }

        return 0;
    }
}
