#if (UNITY_EDITOR)
using UnityEngine;

[ExecuteInEditMode]
public class OrbitLineDrawer : MonoBehaviour
{
    public int _steps = 1000;
    public float _timestep = 0.1f;

    public bool _physicsTimeStep;
    public bool _relativeToBody;

    public CelestialBody _centralBody;
    public float _lineWidth = 100;
    public bool _ThiccLines;

    public void Update()
    {
        if (!Application.isPlaying)
            DrawOrbits();
    }

    private void DrawOrbits()
    {
        CelestialBody[] bodies = FindObjectsOfType<CelestialBody>();
        VirtualBody[] vBodies = new VirtualBody[bodies.Length];
        Vector3[][] draw = new Vector3[bodies.Length][];
        int refFrameIndex = 0;
        Vector3 refBodyInitPos = Vector3.zero;

        // Initialize virtual bodies (don't want to move the actual bodies)
        for (int i = 0; i < vBodies.Length; i++)
        {
            vBodies[i] = new VirtualBody(bodies[i]);
            draw[i] = new Vector3[_steps];

            if (bodies[i] == _centralBody && _relativeToBody)
            {
                refFrameIndex = i;
                refBodyInitPos = vBodies[i].position;
            }
        }
        Simulate(vBodies, refBodyInitPos, draw, refFrameIndex);
        RenderLines(vBodies, draw, bodies);
    }

    private void Simulate(VirtualBody[] vBodies, Vector3 refBodyInitPos, Vector3[][] draw, int frameIndex)
    {
        for (int step = 0; step < _steps; step++)
        {
            Vector3 referenceBodyPosition = (_relativeToBody) ? vBodies[frameIndex].position : Vector3.zero;
            // Update velocities
            for (int i = 0; i < vBodies.Length; i++)
            {
                vBodies[i].velocity += CalculateAcceleration(i, vBodies) * _timestep;
            }
            // Update positions
            for (int i = 0; i < vBodies.Length; i++)
            {
                Vector3 newPos = vBodies[i].position + vBodies[i].velocity * _timestep;
                vBodies[i].position = newPos;
                if (_relativeToBody)
                {
                    var referenceFrameOffset = referenceBodyPosition - refBodyInitPos;
                    newPos -= referenceFrameOffset;
                }
                if (_relativeToBody && i == frameIndex)
                {
                    newPos = refBodyInitPos;
                }

                draw[i][step] = newPos;
            }
        }

    }

    private void RenderLines(VirtualBody[] vBodies, Vector3[][] draw, CelestialBody[] bodies)
    {
        for (int bodyIndex = 0; bodyIndex < vBodies.Length; bodyIndex++)
        {
            var pathColour = bodies[bodyIndex].gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.color; //

            if (_ThiccLines)
            {
                var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
                lineRenderer.enabled = true;
                lineRenderer.positionCount = draw[bodyIndex].Length;
                lineRenderer.SetPositions(draw[bodyIndex]);
                lineRenderer.startColor = pathColour;
                lineRenderer.endColor = pathColour;
                lineRenderer.widthMultiplier = _lineWidth;
            }
            else
            {
                for (int i = 0; i < draw[bodyIndex].Length - 1; i++)
                {
                    Debug.DrawLine(draw[bodyIndex][i], draw[bodyIndex][i + 1], pathColour);
                }

                // Hide renderer
                var lineRenderer = bodies[bodyIndex].gameObject.GetComponentInChildren<LineRenderer>();
                if (lineRenderer)
                {
                    lineRenderer.enabled = false;
                }
            }

        }

    }

    Vector3 CalculateAcceleration(int i, VirtualBody[] vBodies)
    {
        Vector3 acceleration = Vector3.zero;
        for (int j = 0; j < vBodies.Length; j++)
        {
            if (i == j)
            {
                continue;
            }
            Vector3 forceDir = (vBodies[j].position - vBodies[i].position).normalized;
            float sqrDst = (vBodies[j].position - vBodies[i].position).sqrMagnitude;
            acceleration += forceDir * Universe.GRAVITATIONAL_CONSTANT * vBodies[j].mass / sqrDst;
        }
        return acceleration;
    }



    class VirtualBody
    {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;

        public VirtualBody(CelestialBody body)
        {
            position = body.transform.position;
            velocity = body._initialVelocity;
            mass = body._mass;
        }
    }

}
#endif
