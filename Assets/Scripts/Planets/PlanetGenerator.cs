using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlanetGenerator : MonoBehaviour
{

    private SphereMeshGenerator.MeshShape sphereMesh;

    public PlanetShape ShapeGenerator;

    public int qualityLevel = 1;
    private int oldQual = 0;

    public void Awake()
    {
        Generate();
    }

    public void Generate()
    {
        if(oldQual == qualityLevel && sphereMesh.Vertices != null)
        {
            ApplyOffsets();
            sphereMesh.UpdateMeshVertices(GetComponent<MeshFilter>().sharedMesh);
            return;
        }
        sphereMesh = SphereMeshGenerator.CreateNewSphereMeshShape(qualityLevel);
        sphereMesh.GenerateMesh(GetComponent<MeshFilter>().sharedMesh);
        ApplyOffsets();
        sphereMesh.UpdateMeshVertices(GetComponent<MeshFilter>().sharedMesh);
        oldQual = qualityLevel;
    }

    public void ApplyOffsets()
    {
        sphereMesh.Reset();

        ShapeGenerator.ApplyShape(sphereMesh);
    }

}