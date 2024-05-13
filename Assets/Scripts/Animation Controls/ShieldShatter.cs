using System.Security.Cryptography;
using UnityEngine;

public class ShieldShatter : MonoBehaviour
{
    [SerializeField]
    private Light pointLight;

    [SerializeField]
    private ParticleSystem particleSystem;


    private void Update()
    {
        if (pointLight == null) return;
        if (pointLight.range > 0)
        {
            pointLight.range -= 2 * Time.deltaTime;
        }

        if (pointLight.intensity > 0)
        {
            pointLight.intensity -= 1 * Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }

    }
}