using UnityEngine;

[CreateAssetMenu()]
public class ColorSettings : ScriptableObject
{
    public Color _planetColor; // might change to "_color" later


    public Color PlanetColor { get { return _planetColor; } }
}
