using UnityEngine;

[CreateAssetMenu (menuName = "Celestial Body/Settings Holder")]
public class PlanetSettings : ScriptableObject {
	public PlanetBodyShape shape;
	public PlanetShading shading;
}
