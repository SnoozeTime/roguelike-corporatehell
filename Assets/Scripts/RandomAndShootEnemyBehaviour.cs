using UnityEngine;
using System.Collections;
/*
  This enemy is walking to random directions, then shoot in its direction
*/
public class RandomAndShootEnemyBehaviour: ControllerBehaviour {

    // Where to go
    private Vector2 target;

    private float positionOffset = 0.3f;
    private float arriveOffset = 0.4f;
    private int randomIteration;

    private bool shouldMove = true;

    void OnEnable() {
        ChooseRandomTarget();
    }

    public override Control GetControls() {
        Control control = new Control();


        // Try to go to the player, but keep distance :D
        float distance = Vector2.Distance(target, transform.position);

        if (shouldMove) {
            if (distance < arriveOffset) {
                control.shouldFire = true;
                shouldMove = false;

                // Then wait a bit and find a new target.
                StartCoroutine(WaitAndChooseTarget());
            } else {
                int compareXCoord = compareWithOffset(target.x,
                                                      transform.position.x,
                                                      positionOffset);
                int compareYCoord = compareWithOffset(target.y,
                                                      transform.position.y,
                                                      positionOffset);

                control.horizontalMovement = compareXCoord;
                control.verticalMovement = compareYCoord;
                // Face the target.
                control.horizontalOrientation = compareXCoord;
                control.verticalOrientation = compareYCoord;
            }
        }
        return control;
    }

    private void ChooseRandomTarget() {
        // To begin with, just choose normal position +- 5.
        target = transform.position;

        if (randomIteration > 0) {
            target.x -= 5;
            randomIteration = 0;
        } else {
            target.x += 5;
            randomIteration = 1;
        }
    }

    private int compareWithOffset(float coord, float otherCoord, float offset) {

        if (coord < otherCoord - offset) {
            return -1;
        } else if (coord > otherCoord + offset) {
            return 1;
        }

        return 0;
    }

    private IEnumerator WaitAndChooseTarget() {
        yield return new WaitForSeconds(3);
        ChooseRandomTarget();
        shouldMove = true;
    }
}
