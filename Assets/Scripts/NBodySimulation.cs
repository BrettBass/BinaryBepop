using UnityEngine;

public class NBodySimulation : MonoBehaviour
{
    CelestialBody[] _bodies;
    static NBodySimulation _instance;

    void Start()
    {
        _bodies = FindObjectsOfType<CelestialBody> ();
        Time.fixedDeltaTime = Universe.PHYSICS_TIME_STEP;
    }

    void FixedUpdate () {
        for (int i = 0; i < _bodies.Length; i++)
        {
            Vector3 acceleration = CalculateAcceleration (_bodies[i].Position, _bodies[i]);
            _bodies[i].UpdateVelocity(acceleration, Universe.PHYSICS_TIME_STEP);
        }
        for (int i = 0; i < _bodies.Length; i++)
            _bodies[i].UpdatePosition(Universe.PHYSICS_TIME_STEP);
    }

    public static Vector3 CalculateAcceleration (Vector3 point, CelestialBody ignoreBody = null) {

        Vector3 acceleration = Vector3.zero;

        foreach(var body in Instance._bodies) {
            if (body != ignoreBody) {
                float sqrDistance = (body.Position - point).sqrMagnitude;
                Vector3 forceDirection = (body.Position - point).normalized;

                acceleration += forceDirection * Universe.GRAVITATIONAL_CONSTANT * body._mass / sqrDistance;
            }
        }

        return acceleration;
    }
    public static CelestialBody[] Bodies { get { return Instance._bodies; }}
    static NBodySimulation Instance { get { return _instance != null ? _instance : FindObjectOfType<NBodySimulation>(); }}
}
