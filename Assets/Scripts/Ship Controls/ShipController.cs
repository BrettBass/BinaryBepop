using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ShipController : MonoBehaviour
{
    [SerializeField]
    ShipInputControls InputControls;


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
    float RollAmount = 0f;

    [SerializeField]
    [Range(-1f, 1f)]
    float YawAmount = 0f;

    [SerializeField]
    [Range(-1f, 1f)]
    float PitchAmount = 0f;

    [Header("Ship Components")]
    [SerializeField]
    List<ShipEngine> Engines;

    [SerializeField]
    List<Laser> Lasers;

    [SerializeField]
    private AnimateCockpitControls CockpitAnimationControls;

    Rigidbody rb;

    MovementControlsInterface MovementInput => InputControls.movementControls;
    WeaponControlsInterface WeaponInput => InputControls.weaponControls;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

     void Start()
    {
        foreach (ShipEngine engine in Engines)
        {
            engine.Init(MovementInput, rb, ThrustForce / Engines.Count);
        }

        foreach (Laser laser in Lasers)
        {
            //laser.Init(weaponInput, _shipData.BlasterCooldown, _shipData.BlasterLaunchForce, _shipData.BlasterProjectileDuration, _shipData.BlasterDamage);
        }

        if (CockpitAnimationControls != null)
        {
            CockpitAnimationControls.Init(MovementInput);
        }
    }
    void Update()
    {
        YawAmount = MovementInput.YawAmount;
        RollAmount = MovementInput.RollAmount;
        PitchAmount = MovementInput.PitchAmount;
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

       
    }
}
