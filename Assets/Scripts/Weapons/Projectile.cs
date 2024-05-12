using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    [Range(5000f, 25000f)]
    float LaunchForce = 10000f;

    [SerializeField]
    [Range(10, 1000)]
    int Damage = 100;

    [SerializeField]
    [Range(2f, 10f)]
    float Range = 2f;

    [SerializeField] 
    private GameObject explosionPrefab; // Reference to the explosion prefab

    Rigidbody rb;
    float Duration;
    bool OutOfFuel
    {
        get
        {
            Duration -= Time.deltaTime;
            return Duration <= 0f;
        }
    }

   

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        rb.AddForce(LaunchForce * transform.forward);
        Duration = Range;
    }

    void Update()
    {
        if (OutOfFuel) Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        DamageInterface DamageTaken = collision.collider.gameObject.GetComponent<DamageInterface>();
        Vector3 hitPosition = collision.GetContact(0).point;
        if (DamageTaken != null)
        {
            DamageTaken.TakeDamage(Damage, hitPosition);
        }

        // Instantiate the explosion prefab at the collision point
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, hitPosition, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}