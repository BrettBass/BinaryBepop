using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{
    public float _planetRadius = 1;
    public NoiseLayer[] _noiseLayers;

    [System.Serializable]
    public class NoiseLayer
    {
        public bool enabled = true;
        public bool mask;
        public NoiseSettings noiseSettings;
    }
    public NoiseLayer[] NoiseLayers { get { return _noiseLayers; } }
}
