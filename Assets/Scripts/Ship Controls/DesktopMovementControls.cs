using System;
using UnityEngine;

[Serializable]
public class DesktopMovementControls : MovementControlsBase
{
    [SerializeField]
    float DeadZoneRadius = 0.1f;
    float RollAmt = 0f;
    Vector2 ScreenCenter => new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

    public override float ThrustAmount => Input.GetAxis("Vertical");
  

    public override float YawAmount 
    {
        get
        {
            Vector3 MousePosition = Input.mousePosition;
            float Yaw = (MousePosition.x - ScreenCenter.x) / ScreenCenter.x;
            return Mathf.Abs(Yaw) > DeadZoneRadius ? Yaw : 0f;
        }
    }

    public override float PitchAmount 
    {
        get
        {
            Vector3 MousePosition = Input.mousePosition;
            float Pitch = (MousePosition.y - ScreenCenter.y) / ScreenCenter.y;
            return Mathf.Abs(Pitch) > DeadZoneRadius ? Pitch * -1f : 0f;
        }
    }
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

            RollAmt = Mathf.Lerp(RollAmt, roll, Time.deltaTime * 3f);
            return RollAmt;
        }
    }

   
}
