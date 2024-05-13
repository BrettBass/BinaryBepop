using System.Collections;
using System.Collections.Generic;
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

    bool CanFire
    {
        get
        {
            Cooldown -= Time.deltaTime;
            return Cooldown <= 0f;
        }
    }

    float Cooldown;

    // Update is called once per frame
    void Update()
    {
        if (CanFire && Input.GetMouseButton(0))
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        Cooldown = CooldownTime;
        Instantiate(ProjectilePrefab, Muzzle.position, transform.rotation);
    }
}