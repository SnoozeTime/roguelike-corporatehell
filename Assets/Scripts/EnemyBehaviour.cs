using UnityEngine;
using System;
using System.Collections.Generic;
/*
  Enemy behaviour is a combinaison of different controller behaviour (such as follow player,
  avoid obstacle or more enemy type specific behaviour). It's an implementation of the
  AI steering behaviour pipeline that was described in AI in game programming book.
  In our case, we do not need an actuator as we are directly setting the position.

  The list of behaviour for each enemy will be read from the disk.
*/
public class EnemyBehaviour: ControllerBehaviour {

    [Serializable]
    public class WeightedBehaviour {
        public ControllerBehaviour behaviour;
        public float weight;
    }

    [SerializeField]
    private List<WeightedBehaviour> controllerComponents;

    private Transform playerTransform;

    private float positionOffset = 0.3f;

    // First moves within 3pixel to the player.
    private float minDistanceToPlayer = 3f;

    public void Start() {
        // need to follow the player.
        //        GameObject[] players = GameObject.FindGameObjectsWithTag(
         //  EnumUtils.StringValueOf(Tags.PLAYER)
        //    );

        // We don't care about checking the length. if no player, the game
        // won't work anyway.
//        playerTransform = players[0].transform;
    }

    public override Control GetControls() {
        // Control control = new Control();

        // // Try to go to the player, but keep distance
        // float distance = Vector2.Distance(playerTransform.position, transform.position);

        // int compareXCoord = compareWithOffset(playerTransform.position.x,
        //                                       transform.position.x,
        //                                       positionOffset);
        // int compareYCoord = compareWithOffset(playerTransform.position.y,
        //                                       transform.position.y,
        //                                       positionOffset);
        // if (distance > minDistanceToPlayer) {
        //     control.horizontalMovement = compareXCoord;
        //     control.verticalMovement = compareYCoord;
        // } else {
        //     // Just adjust X then.
        //     control.horizontalMovement = compareXCoord;
        // }
        // // Face the player.
        // control.horizontalOrientation = compareXCoord;
        // control.verticalOrientation = compareYCoord;

        // // if same line/column, shoot.
        // control.shouldFire = compareXCoord == 0 || compareYCoord == 0;

        return BlendControllerBehaviours();
    }

    /*
      For now, just a weighted average of the controls of different behaviour.
      If blended horizontalMovement < 0.5, round to 0. (majority of behaviour said NO to
      movement)
      If > 0.5, round to 1.

      Later some behaviour might only be here for shooting. In that case, we will ignore them.
     */
    private Control BlendControllerBehaviours() {
        Debug.Log("WIll blend behaviours");
        // The blended values. They are float that will be later rounded to int.
        float blendedHMvt = 0f;
        float blendedVMvt = 0f;
        float blendedHOrientation = 0f;
        float blendedVOrientation = 0f;
        float blendedFire = 0f;

        float weightSum = 0f;
        foreach (WeightedBehaviour wb in controllerComponents) {
            blendedHMvt += wb.behaviour.GetControls().horizontalMovement * wb.weight;
            blendedVMvt += wb.behaviour.GetControls().verticalMovement * wb.weight;
            blendedHOrientation += wb.behaviour.GetControls().horizontalOrientation * wb.weight;
            blendedVOrientation += wb.behaviour.GetControls().verticalOrientation * wb.weight;

            if (wb.behaviour.GetControls().shouldFire) {
                blendedFire += wb.weight;
            }

            Debug.Log("-------------");
            Debug.Log(wb.behaviour.GetControls().horizontalMovement);
            Debug.Log(wb.behaviour.GetControls().verticalMovement);
            Debug.Log(wb.behaviour.GetControls().horizontalOrientation);
            Debug.Log(wb.behaviour.GetControls().verticalOrientation);
            Debug.Log("-------------");


            weightSum += wb.weight;
        }

        // put back the values between 0 and 1.
        if (weightSum != 0) {
            blendedHMvt /= weightSum;
            blendedVMvt /= weightSum;
            blendedHOrientation /= weightSum;
            blendedVOrientation /= weightSum;
            blendedFire /= weightSum;
        }

        // Then, assign to Control
        // TODO Value can be either -1, 0 or 1...
        Control control = new Control();
        control.horizontalMovement = (int) (blendedHMvt + 0.5);
        control.verticalMovement = (int) (blendedVMvt + 0.5);
        control.horizontalOrientation = (int) (blendedHOrientation + 0.5);
        control.verticalOrientation = (int) (blendedVOrientation + 0.5);
        control.shouldFire = blendedFire >= 0.5;

        Debug.Log("----- BLENDED VALUES --------");
        Debug.Log(control.horizontalMovement);
        Debug.Log(control.verticalMovement);
        Debug.Log(control.horizontalOrientation);
        Debug.Log(control.verticalOrientation);
        Debug.Log("-------------");


        return control;
    }
}
