using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PlanetShape : ScriptableObject
{
    public virtual void ApplyShape(SphereMeshGenerator.MeshShape mesh) { 
        for(int i = 0; i < mesh.Vertices.Length; i++)
        {
            mesh.Vertices[i] = ModifyVertex(mesh.Vertices[i]);
        }
    }
    
    public virtual Vector3 ModifyVertex(Vector3 v)
    {
        return v;
    }
}
