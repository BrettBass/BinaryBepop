using System;
using UnityEngine;


// Serializable class for movement controls, inheriting from MovementControlsBase.
[Serializable]
public class MovementControls : MovementControlsBase
{
    // Serialized field to define the dead zone radius for mouse input.
    [SerializeField]
    float DeadZoneRadius = 0.1f;
    float RollAmt = 0f;
    Vector2 ScreenCenter => new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

   
    // Override property to get the thrust amount from the W key press.
    public override float ThrustAmount
    {
        get
        {
            // Check if the W key is pressed.
            bool isWPressed = Input.GetKey(KeyCode.W);

            // Return 1 if W is pressed, otherwise return 0.
            return isWPressed ? 1 : 0;
        }
    }

    // Override property to get the yaw amount based on mouse position relative to the screen center.
    public override float YawAmount 
    {
        get
        {
            Vector3 MousePosition = Input.mousePosition;
            float Yaw = (MousePosition.x - ScreenCenter.x) / ScreenCenter.x;
            return Mathf.Abs(Yaw) > DeadZoneRadius ? Yaw : 0f;
        }
    }

    // Override property to get the pitch amount
    public override float PitchAmount 
    {
        get
        {
            Vector3 MousePosition = Input.mousePosition;
            float Pitch = (MousePosition.y - ScreenCenter.y) / ScreenCenter.y;
            return Mathf.Abs(Pitch) > DeadZoneRadius ? Pitch * -1f : 0f;
        }
    }

    // Override property to get the roll amount
    public override float RollAmount 
    {
        get
        {
            float roll;
            if (Input.GetKey(KeyCode.Q))
            {
                roll = 1f;
            }

            else
            {
                roll = Input.GetKey(KeyCode.E) ? -1f : 0f;
            }

            // interpolate for smoothness the current amount towards the new roll
            RollAmt = Mathf.Lerp(RollAmt, roll, Time.deltaTime * 3f);
            return RollAmt;
        }
    }

   
}
