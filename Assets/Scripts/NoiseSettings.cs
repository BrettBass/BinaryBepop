using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public float Strength = 1f;
    public float Roughness = 2f;
    public Vector3 Center;
    [Range(1,10)]
    public int NumLayers = 1;
    public float Persistence = 0.5f;
    public float BaseRoughness = 1f;
    public float MinValue;
    [Range(0,256)]
    public int seed = 0;
}
