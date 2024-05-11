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

    void Update()
    {
        ActivateThrusters();
    }

    void FixedUpdate()
    {
        if (!ThrustersEnabled) return;
        ridgidb.AddForce(transform.forward * ThrustAmount * Time.fixedDeltaTime);
    }

    public void Init(MovementControlsInterface MovementControls, Rigidbody rb, float thrustForce)
    {
        ShipMovementControls = MovementControls;
        ridgidb = rb;
        ThrustForce = thrustForce;
    }

    void ActivateThrusters()
    {
        Thruster.SetActive(ThrustersEnabled);
        if (!ThrustersEnabled) return;
        ThrustAmount = ThrustForce * ShipMovementControls.ThrustAmount;
    }

}

