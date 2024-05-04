using UnityEngine;

public class NoiseFilter : MonoBehaviour
{
    Noise _noise = new Noise();
    NoiseSettings _settings;

    public NoiseFilter(NoiseSettings settings)
    {
        _settings = settings;
        _noise = new Noise(settings.seed);
    }
    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float freq = _settings.BaseRoughness;
        float amp = 1;

        for (int i = 0; i < _settings.NumLayers; i++)
        {
            float val = _noise.Evaluate(point * freq + _settings.Center);
            noiseValue += (val + 1) * 0.5f * amp;
            freq *= _settings.Roughness;
            amp *= _settings.Persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - _settings.MinValue);
        return noiseValue * _settings.Strength;
    }
}
