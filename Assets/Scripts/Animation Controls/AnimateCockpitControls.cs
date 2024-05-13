using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateCockpitControls : MonoBehaviour
{
    [Header("Controls Transforms")]
    [SerializeField]
    Transform Joystick;

    [SerializeField]
    Vector3 JoystickRange = Vector3.zero;

    [SerializeField]
    List<Transform> Throttles;

    [SerializeField]
    float ThrottleRange = 35f;

    [SerializeField]
    ShipInputControls movementInput;

    [SerializeField]
    MovementControlsInterface ControlInput => movementInput.movementControls;

    // Update is called once per frame
    void Update()
    {
        Joystick.localRotation = Quaternion.Euler(ControlInput.PitchAmount * JoystickRange.x, ControlInput.YawAmount * JoystickRange.y, ControlInput.RollAmount * JoystickRange.z);

        Vector3 ThrottleRotation = Throttles[0].localRotation.eulerAngles;
        ThrottleRotation.x = ControlInput.ThrustAmount * ThrottleRange;
        foreach(Transform t in Throttles) 
        {
            t.localRotation = Quaternion.Euler(ThrottleRotation);
        }
    }
}
