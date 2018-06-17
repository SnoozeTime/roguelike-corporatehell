using UnityEngine;

/*
  Abstract class that provides the decision to move or fire.
  For players, this is implemented using the inputs.
*/
public abstract class ControllerBehaviour: MonoBehaviour {
    public abstract Control GetControls();
}
