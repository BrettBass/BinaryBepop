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

    public static MovementControlsInterface GetInputControls(InputType type)
    {
        return type switch
        {
            InputType.Desktop => new DesktopMovementControls(),
            InputType.Controller => null,
            InputType.Bot => null,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
