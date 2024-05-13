using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Shield : MonoBehaviour, DamageInterface
{
    [SerializeField]
    private Color flashColor = Color.white;

    [SerializeField]
    [Range(0.25f, 1f)]
    private float fadeOutTime = 0.5f;

    [SerializeField]
    private float minIntensity = -10f, maxIntensity = 0f;

    [SerializeField]
    private int maxHealth = 5000;

    [SerializeField]
    private GameObject ExplosionPrefab;

    private Renderer renderer;
    private Color BaseColor;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private int Health;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        Health = maxHealth;
        BaseColor = renderer.material.color;
    }

    public void TakeDamage(int damage, Vector3 hitPosition)
    {
        Health -= damage;
        if (Health <= 0)
        {
            DestroyShields();
            return;
        }
        StartCoroutine(FlashAndFadeShields());
    }

    private void DestroyShields()
    {
        StopAllCoroutines();
        if (ExplosionPrefab != null)
        {
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    IEnumerator FlashAndFadeShields()
    {
        Color shieldColor;
        Color emissionColor = renderer.material.GetColor(EmissionColor);
        float currentTime = 0f;
        float intensity = maxIntensity;
        while (currentTime < fadeOutTime)
        {
            shieldColor = Color.Lerp(flashColor, BaseColor, currentTime / fadeOutTime);
            intensity = Mathf.Lerp(maxIntensity, minIntensity, currentTime / fadeOutTime);
            ChangeShieldColor(shieldColor, emissionColor * Mathf.Pow(2, intensity));
            currentTime += Time.deltaTime;
            yield return null;
        }

        yield break;
    }

    private void ChangeShieldColor(Color shieldColor, Color emissionColor)
    {
        renderer.material.color = shieldColor;
        renderer.material.SetColor(EmissionColor, emissionColor);
    }
}