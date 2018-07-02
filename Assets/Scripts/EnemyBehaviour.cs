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

    public override Control GetControls() {
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
        // The blended values. They are float that will be later rounded to int.
        float blendedHMvt = 0f;
        float blendedVMvt = 0f;
        float blendedHOrientation = 0f;
        float blendedVOrientation = 0f;
        float blendedFire = 0f;
        float blendedNextWeapon = 0f;
        Vector2 blendedAttackDirection = new Vector2();
        float weightMvtSum = 0f;
        float weightAtkSum = 0f;
        foreach (WeightedBehaviour wb in controllerComponents) {

            // Important here! Just get the control once in case it trigger side effects
            // or state machine transitions...
            Control behaviourControl = wb.behaviour.GetControls();

            // ---------------------------------------------------------
            // 1. BLEND MOVEMENT
            // ---------------------------------------------------------

            if (behaviourControl.movementControl != null) {
                blendedHMvt += behaviourControl.movementControl.horizontalMovement * wb.weight;
                blendedVMvt += behaviourControl.movementControl.verticalMovement * wb.weight;
                blendedHOrientation += behaviourControl.movementControl.horizontalOrientation * wb.weight;
                blendedVOrientation += behaviourControl.movementControl.verticalOrientation * wb.weight;
                weightMvtSum += wb.weight;
            }

            // ---------------------------------------------------------
            // 2. BLEND ATTACK
            // ---------------------------------------------------------
            if (behaviourControl.attackControl != null) {
                blendedNextWeapon += behaviourControl.attackControl.selectNextWeapon * wb.weight;
                blendedAttackDirection += behaviourControl.attackControl.direction * wb.weight;

                if (behaviourControl.attackControl.shouldFire) {
                    blendedFire += wb.weight;
                }

                weightAtkSum += wb.weight;
            }
        }

        // put back the values between 0 and 1.
        if (weightMvtSum != 0) {
            blendedHMvt /= weightMvtSum;
            blendedVMvt /= weightMvtSum;
            blendedHOrientation /= weightMvtSum;
            blendedVOrientation /= weightMvtSum;
        }

        if (weightAtkSum != 0) {
            blendedFire /= weightAtkSum;
            blendedNextWeapon /= weightAtkSum;
            blendedAttackDirection /= weightAtkSum;
        }
        // Then, assign to Control
        // TODO Value can be either -1, 0 or 1...
        Control control = new Control();

        MovementControl movementControl = new MovementControl();
        movementControl.horizontalMovement = (int) Math.Round(blendedHMvt, MidpointRounding.AwayFromZero);
        movementControl.verticalMovement = (int) Math.Round(blendedVMvt, MidpointRounding.AwayFromZero);
        movementControl.horizontalOrientation = (int) Math.Round(blendedHOrientation, MidpointRounding.AwayFromZero);
        movementControl.verticalOrientation = (int) Math.Round(blendedVOrientation, MidpointRounding.AwayFromZero);

        AttackControl attackControl = new AttackControl();
        attackControl.shouldFire = blendedFire >= 0.5;
        attackControl.selectNextWeapon = (int) Math.Round(blendedNextWeapon, MidpointRounding.AwayFromZero);
        attackControl.direction = new Vector2((int) Math.Round(blendedAttackDirection.x, MidpointRounding.AwayFromZero),
                                              (int) Math.Round(blendedAttackDirection.y, MidpointRounding.AwayFromZero));

        control.movementControl = movementControl;
        control.attackControl = attackControl;
        return control;
    }
}
