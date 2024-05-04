using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int _resolution = 10;

    // currently working on
    public ShapeSettings _shapeSettings;
    public ColorSettings _colorSettings;
    public ShapeGenerator _shapeGenerator;

    public bool _autoUpdate = true;
    [HideInInspector] public bool ShapeSettingsAccordion;
    [HideInInspector] public bool ColorSettingsAccordion;

    [SerializeField, HideInInspector]
    MeshFilter[] _meshFilters;
    TerrainFace[] _terrainFaces;

    private void OnValidate()
    {
        GeneratePlanet();
    }

    void Initialize()
    {
        _shapeGenerator = new ShapeGenerator(_shapeSettings);
        if (_meshFilters == null || _meshFilters.Length == 0)
            _meshFilters = new MeshFilter[6];
        _terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (_meshFilters[i] == null)
            {
                GameObject mObj = new GameObject("mesh");
                mObj.transform.parent = transform;

                mObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                _meshFilters[i] = mObj.AddComponent<MeshFilter>();
                _meshFilters[i].sharedMesh = new Mesh();
            }

            _terrainFaces[i] = new TerrainFace(_shapeGenerator, _meshFilters[i].sharedMesh, _resolution, directions[i]);
        }
    }

    private void GenerateMesh()
    {
        foreach (TerrainFace face in _terrainFaces)
        {
            face.ConstructMesh();
        }
    }

    private void GenerateColors()
    {
        foreach (MeshFilter m in _meshFilters)
        {
            m.GetComponent<MeshRenderer>().sharedMaterial.color = _colorSettings.PlanetColor;
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }
    public void OnShapeSettingsUpdate()
    {
        if (_autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }
    public void OnColorSettingsUpdate()
    {
        if (_autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }
    public ShapeSettings ShapeSettings { get { return _shapeSettings; } }
    public ColorSettings ColorSettings { get { return _colorSettings; } }
}
