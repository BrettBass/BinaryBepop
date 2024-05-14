using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEngine : MonoBehaviour
{
    // Serialized field to assign the thruster GameObject in the Unity Editor.
    [SerializeField] GameObject Thruster;

    // Interface for controlling ship movement.
    MovementControlsInterface ShipMovementControls;

    // Rigidbody component for applying forces.
    Rigidbody rigidb;

    // Force od thrust applied to the ship.
    float ThrustForce;

    // Amount of thrust to apply to the ship.
    float ThrustAmount;

    // Boolean thrusters should be enabled based on the thrust amount.
    bool ThrustersEnabled => !Mathf.Approximately(0f, ShipMovementControls.ThrustAmount);

    // Initialization method to intialize the ship engine with movement controls, Rigidbody, and thrust force.
    public void Init(MovementControlsInterface movementControls, Rigidbody rb, float thrustForce)
    {
        ShipMovementControls = movementControls;
        rigidb = rb;
        ThrustForce = thrustForce;
    }

    // Thrusters called every frame
    void Update()
    {
        ActivateThrusters();
    }

    // FixedUpdate calls once per physics update to apply thrust
    void FixedUpdate()
    {
        // exit the method early if thrusters are not enabled
        if (!ThrustersEnabled) return;

        // Apply forward force to the Rigidbody based on the thrust amount multipled by deltatime.
        rigidb.AddForce(transform.forward * ThrustAmount * Time.fixedDeltaTime);
    }

    // Method to activate or deactivate the thrusters based on the ThrustersEnabled property.
    void ActivateThrusters()
    {
      
        Thruster.SetActive(ThrustersEnabled);
        if (!ThrustersEnabled) return;
        ThrustAmount = ThrustForce * ShipMovementControls.ThrustAmount;
    }
}

