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
    private MovementControlsInterface _movementInput;

    // Update is called once per frame
    void Update()
    {
        if (_movementInput == null) return;
             Joystick.localRotation = Quaternion.Euler(
            _movementInput.PitchAmount * JoystickRange.x,
            _movementInput.YawAmount * JoystickRange.y,
            _movementInput.RollAmount * JoystickRange.z
        );

        Vector3 throttleRotation = Throttles[0].localRotation.eulerAngles;
        throttleRotation.x = _movementInput.ThrustAmount * ThrottleRange;
        foreach (Transform throttle in Throttles)
        {
            throttle.localRotation = Quaternion.Euler(throttleRotation);
        }
    }

    public void Init(MovementControlsInterface movementControls)
    {
        _movementInput = movementControls;
    }
}
