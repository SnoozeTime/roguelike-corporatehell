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

        float weightSum = 0f;
        foreach (WeightedBehaviour wb in controllerComponents) {
            blendedHMvt += wb.behaviour.GetControls().horizontalMovement * wb.weight;
            blendedVMvt += wb.behaviour.GetControls().verticalMovement * wb.weight;
            blendedHOrientation += wb.behaviour.GetControls().horizontalOrientation * wb.weight;
            blendedVOrientation += wb.behaviour.GetControls().verticalOrientation * wb.weight;

            if (wb.behaviour.GetControls().shouldFire) {
                blendedFire += wb.weight;
            }

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
        control.horizontalMovement = (int) Math.Round(blendedHMvt, MidpointRounding.AwayFromZero);
        control.verticalMovement = (int) Math.Round(blendedVMvt, MidpointRounding.AwayFromZero);
        control.horizontalOrientation = (int) Math.Round(blendedHOrientation, MidpointRounding.AwayFromZero);
        control.verticalOrientation = (int) Math.Round(blendedVOrientation, MidpointRounding.AwayFromZero);
        control.shouldFire = blendedFire >= 0.5;

        return control;
    }
}
