using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    Projectile ProjectilePrefab;

    [SerializeField]
    Transform Muzzle;

    [SerializeField]
    [Range(0f, 5f)]
    float CooldownTime = 0.25f;
    float Cooldown;
    int LaunchForce;
    int Damage;
    float Duration;

    WeaponControlsInterface WeaponInput;

    bool CanFire
    {
        get
        {
            Cooldown -= Time.deltaTime;
            return Cooldown <= 0f;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (WeaponInput == null) return;
        if (CanFire && WeaponInput.PrimaryFired)
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        Cooldown = CooldownTime;
        Projectile projectile = Instantiate(ProjectilePrefab, Muzzle.position, transform.rotation);
        projectile.gameObject.SetActive(false);
        projectile.Init(LaunchForce, Damage, Duration);
        projectile.gameObject.SetActive(true);
    }

    public void Init(WeaponControlsInterface weaponInput, float CoolDown, int launchForce, float duration, int damage)
    {
        WeaponInput = weaponInput;
        CooldownTime = CoolDown;
        LaunchForce = launchForce;
        Duration = duration;
        Damage = damage;
    }
}