using UnityEngine;

public class ShipInputControls : MonoBehaviour
{
    [SerializeField] ShipInputManager.InputType inputType = ShipInputManager.InputType.Desktop;

    public MovementControlsInterface movementControls { get; private set; }
    public WeaponControlsInterface weaponControls { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        //Fetches appropriate input controls for whatever object the script is placed on.
        movementControls = ShipInputManager.GetMovementControls(inputType);
        weaponControls = ShipInputManager.GetWeaponControls(inputType);
    }

    void OnDestroy()
    {
       movementControls = null;
    }
}
