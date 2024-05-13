using System;
using UnityEngine;

public class ShipInputManager : MonoBehaviour
{
    public enum InputType
    {
        Desktop,
        Controller,
        Bot
    }

    public static MovementControlsInterface GetMovementControls(InputType type)
    {
        return type switch
        {
            InputType.Desktop => new MovementControls(),
            InputType.Controller => null,
            InputType.Bot => null,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static WeaponControlsInterface GetWeaponControls(InputType inputType)
    {
        return inputType switch
        {
            InputType.Desktop => new WeaponControls(),
            InputType.Controller => null,
            InputType.Bot => null,
            _ => throw new ArgumentOutOfRangeException(nameof(inputType), inputType, null)
        };
    }
}
