using System;
using UnityEngine;

public class ShipInputManager : MonoBehaviour
{
    // Enum to represent different types of inputs.
    public enum InputType
    {
        Desktop,
        Controller,
        Bot
    }

    // Static method to get movement controls based on the input type.
    public static MovementControlsInterface GetMovementControls(InputType type)
    {
        //Switch expression similar to switch case
        return type switch
        {
            InputType.Desktop => new MovementControls(),
            InputType.Controller => null,
            InputType.Bot => null,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    // Static method to get weapon controls based on the input type.
    public static WeaponControlsInterface GetWeaponControls(InputType inputType)
    {
        //Switch expression similar to switch case
        return inputType switch
        {
            InputType.Desktop => new WeaponControls(),
            InputType.Controller => null,
            InputType.Bot => null,
            _ => throw new ArgumentOutOfRangeException(nameof(inputType), inputType, null)
        };
    }
}
