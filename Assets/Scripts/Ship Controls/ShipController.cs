using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField]
    ShipMovementInput MovementInput;


    [Header("Ship Movement Values")]
    [SerializeField]
    [Range(1000f, 10000f)]
    float ThrustForce = 7500f;

    [SerializeField]
    [Range(1000f, 10000f)]
    float RollForce = 1000f;

    [SerializeField]
    [Range(1000f, 10000f)]
    float YawForce = 2000f;

    [SerializeField]
    [Range(1000f, 10000f)]
    float PitchForce = 6000f;

    [SerializeField]
    [Range(-1f, 1f)]
    float ThrustAmount = 0f;

    [SerializeField]
    [Range(-1f, 1f)]
    float RollAmount = 0f;

    [SerializeField]
    [Range(-1f, 1f)]
    float YawAmount = 0f;

    [SerializeField]
    [Range(-1f, 1f)]
    float PitchAmount = 0f;

    Rigidbody rb;

    MovementControlsInterface ControlInput => MovementInput.movementControls;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ThrustAmount = ControlInput.ThrustAmount;
        YawAmount = ControlInput.YawAmount;
        RollAmount = ControlInput.RollAmount;
        PitchAmount = ControlInput.PitchAmount;
    }

    void FixedUpdate()
    {
        if(!Mathf.Approximately(0f, PitchAmount))
        {
            rb.AddTorque(transform.right * (PitchForce * PitchAmount * Time.fixedDeltaTime));
        }

        if (!Mathf.Approximately(0f, RollAmount))
        {
            rb.AddTorque(transform.forward * (RollForce * RollAmount * Time.fixedDeltaTime));
        }

        if (!Mathf.Approximately(0f, YawAmount))
        {
            rb.AddTorque(transform.up * (YawForce * YawAmount * Time.fixedDeltaTime));
        }

        if (!Mathf.Approximately(0f, ThrustAmount))
        {
            rb.AddForce(transform.forward * (ThrustForce * ThrustAmount * Time.fixedDeltaTime));
        }
    }
}
