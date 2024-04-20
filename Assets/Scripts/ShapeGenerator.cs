using UnityEngine;
public class ShapeGenerator
{
    ShapeSettings _settings;
    NoiseFilter[] _noiseFilters;

    public ShapeGenerator(ShapeSettings s)
    {
        _settings = s;
        _noiseFilters = new NoiseFilter[s.NoiseLayers.Length];
        for(int i = 0; i < _noiseFilters.Length; i++)
            _noiseFilters[i] = new NoiseFilter(s.NoiseLayers[i].noiseSettings);

        //_noiseFilter = new NoiseFilter(_settings._noiseSettings);
    }

    public Vector3 CalculatePointOnPlanet(Vector3 point)
    {
        float firstLayerValue = 0;
        float elevation = 0;
        //float elevation = _noiseFilter.Evaluate(point);
        if (_noiseFilters.Length > 0)
        {
            firstLayerValue = _noiseFilters[0].Evaluate(point);
            if (_settings._noiseLayers[0].enabled)
                elevation = firstLayerValue;
        }
        for(int i = 1; i < _noiseFilters.Length; i++)
        {
            if(_settings._noiseLayers[i].enabled)
            {
                float mask = (_settings._noiseLayers[i].mask) ? firstLayerValue : 1;
                elevation += _noiseFilters[i].Evaluate(point) * mask;
            }
        }
        return point * _settings._planetRadius * (1 + elevation); // bootleg normalize, will customize later
    }

}
