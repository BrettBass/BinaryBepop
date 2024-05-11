using UnityEngine;

public class ShipMovementInput : MonoBehaviour
{
    [SerializeField] ShipInputManager.InputType inputType = ShipInputManager.InputType.Desktop;

    public MovementControlsInterface movementControls {  get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        //Fetches appropriate input controls for whatever object the script is placed on.
        movementControls = ShipInputManager.GetInputControls(inputType);
    }

    void OnDestroy()
    {
       movementControls = null;
    }
}
