using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEngine : MonoBehaviour
{
    [SerializeField] GameObject Thruster;

    MovementControlsInterface ShipMovementControls;
    Rigidbody ridgidb;
    float ThrustForce;
    float ThrustAmount;

    bool ThrustersEnabled => !Mathf.Approximately(0f, ShipMovementControls.ThrustAmount);
 public void Init(MovementControlsInterface movementControls, Rigidbody rb, float thrustForce)
    {
        ShipMovementControls = movementControls;
        ridgidb = rb;
        ThrustForce = thrustForce;
    }
    void Update()
    {
        ActivateThrusters();
    }

    void FixedUpdate()
    {
        if (!ThrustersEnabled) return;
        ridgidb.AddForce(transform.forward * ThrustAmount * Time.fixedDeltaTime);
    }

   

    void ActivateThrusters()
    {
        Thruster.SetActive(ThrustersEnabled);
        if (!ThrustersEnabled) return;
        ThrustAmount = ThrustForce * ShipMovementControls.ThrustAmount;
    }

}

