using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int _resolution = 10;

    [SerializeField, HideInInspector]
    MeshFilter[] _meshFilters;
    TerrainFace[] _terrainFaces;

    private void OnValidate()
    {
        Initialize();
        GenerateMesh();
    }

    void Initialize()
    {
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

            _terrainFaces[i] = new TerrainFace(_meshFilters[i].sharedMesh, _resolution, directions[i]);
        }
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in _terrainFaces)
        {
            face.ConstructMesh();
        }
    }
}
