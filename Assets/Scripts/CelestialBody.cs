using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CelestialBody : MonoBehaviour
{

    public float _radius;
    public float _gravity;
    public Vector3 _initialVelocity;
    public string _name = "Unnamed";
    Transform _mesh;

    public Vector3 _velocity { get; private set; }
    public float _mass { get; private set; }
    Rigidbody _rb;

    // using awake rather than start to force initialization
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.mass = _mass;
        _velocity = _initialVelocity;
    }

    public void UpdateVelocity(CelestialBody[] bodies, float timeStep)
    {
        foreach (var otherBody in bodies)
        {
            if (otherBody != this) // can't inflict gravitational pull on self
            {
                float sqrDst = (otherBody._rb.position - _rb.position).sqrMagnitude;
                Vector3 forceDir = (otherBody._rb.position - _rb.position).normalized;

                Vector3 acceleration = forceDir * Universe.GRAVITATIONAL_CONSTANT * otherBody._mass / sqrDst;
                _velocity += acceleration * timeStep;
            }
        }
    }

    // not used atm
    public void UpdateVelocity(Vector3 acceleration, float timeStep)
    {
        _velocity += acceleration * timeStep;
    }

    public void UpdatePosition(float timeStep)
    {
        _rb.MovePosition(_rb.position + _velocity * timeStep);
    }

    void OnValidate()
    {
        _mass = _gravity * _radius * _radius / Universe.GRAVITATIONAL_CONSTANT; // newton's gravity equation
        _mesh = transform.GetChild(0);
        _mesh.localScale = Vector3.one * _radius; // make sure radius it set or else sphere has surfaces with 0 scale
        gameObject.name = _name;
    }


    public Rigidbody Rigidbody { get { return _rb; } }
    public Vector3 Position { get { return _rb.position; } }
}

