using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SphereMeshGenerator
{
    public struct MeshShape
    {
        public Vector3[] Vertices;
        Polygon[] Triangles;

        private Vector3[] _originalVerts;

        public MeshShape(Vector3[] verts, Polygon[] polys)
        {
            Vertices = verts;
            Triangles = polys;
            _originalVerts = verts;
        }

        public void GenerateMesh(Mesh mesh)
        {
            mesh.Clear();

            mesh.vertices = Vertices;

            //Debug.Log($"{Vertices.Length} Vertices Found");

            Vector3[] normals = new Vector3[Vertices.Length];
            for(int i = 0; i < Vertices.Length; i++)
            {
                normals[i] = Vertices[i];
            }

            mesh.normals = normals;

            int[] trisList = new int[Triangles.Length * 3];
            for(int i = 0; i < Triangles.Length; i++)
            {
                var tri = Triangles[i];
                trisList[i * 3] = tri.vertices[0];
                trisList[i * 3 + 1] = tri.vertices[1];
                trisList[i * 3 + 2] = tri.vertices[2];
            }
            mesh.SetTriangles(trisList, 0);
            _originalVerts = new Vector3[Vertices.Length];
            Vertices.CopyTo(_originalVerts, 0);
        }

        public void Reset()
        {
            Vertices = new Vector3[_originalVerts.Length];
            _originalVerts.CopyTo(Vertices, 0);
        }

        public void UpdateMeshVertices(Mesh mesh)
        {
            if(Vertices.Length != mesh.vertices.Length)
            {
                Debug.LogWarning("Mesh Vertices Not Same Length, Use GenerateMesh() Instead");
                GenerateMesh(mesh);
            } else
            {
                mesh.vertices = Vertices;
            }
        }
    }

    public static MeshShape CreateNewSphereMeshShape(int qualityLevel)
    {
        var vertices = new List<Vector3>();
        var polygons = new List<Polygon>();

        vertices.Add(new Vector3(0, 0, 1));
        vertices.Add(new Vector3(0, 0, -1));
        vertices.Add(new Vector3(0, 1, 0));
        vertices.Add(new Vector3(0, -1, 0));
        vertices.Add(new Vector3(1, 0, 0));
        vertices.Add(new Vector3(-1, 0, 0));

        //Top Half
        polygons.Add(new Polygon(2, 0, 4));
        polygons.Add(new Polygon(2, 4, 1));
        polygons.Add(new Polygon(2, 1, 5));
        polygons.Add(new Polygon(2, 5, 0));

        //Bottom Half
        polygons.Add(new Polygon(3, 4, 0));
        polygons.Add(new Polygon(3, 1, 4));
        polygons.Add(new Polygon(3, 5, 1));
        polygons.Add(new Polygon(3, 0, 5));

        Subdivide(ref vertices, ref polygons, qualityLevel);
        PushVerticesToSphere(vertices);
        return new MeshShape(vertices.ToArray(), polygons.ToArray());
    }

    private static void Subdivide(ref List<Vector3> vertices, ref List<Polygon> polygons, int rec)
    {
        var newPolys = new List<Polygon>();
        foreach (var poly in polygons)
        {
            float sideLength = (float)1 / (rec + 1);

            //Creates Side Verticies
            List<int[]> triStruct = new List<int[]>();
            triStruct.Add(new int[] { poly.vertices[0] });
            for (int i = 0; i < rec + 1; i++)
            {
                //i is level down triangle
                int a = poly.vertices[0];
                int b = poly.vertices[1];
                int c = poly.vertices[2];

                Vector3 ab = Vector3.Lerp(vertices[a], vertices[b], sideLength * (i + 1));
                Vector3 ac = Vector3.Lerp(vertices[a], vertices[c], sideLength * (i + 1));

                int[] triLevel = new int[2 + i];
                triLevel[0] = GetVertFromVec3(vertices, ab);

                triLevel[triLevel.Length - 1] = GetVertFromVec3(vertices, ac);

                for (int j = 0; j < i; j++)
                {
                    Vector3 mid = Vector3.Lerp(ab, ac, (float)(j + 1) / (i + 1));
                    triLevel[j + 1] = GetVertFromVec3(vertices, mid);
                }
                triStruct.Add(triLevel);
            }

            //Fill Downwards
            for (int vd = 0; vd < triStruct.Count - 1; vd++)
            {
                int[] level = triStruct[vd];
                int[] below = triStruct[vd + 1];
                for (int va = 0; va < level.Length; va++)
                {
                    int vert = level[va];
                    newPolys.Add(new Polygon(vert, below[va], below[va + 1]));
                }
            }

            //Fill Upwards
            for (int vu = triStruct.Count - 1; vu > 0; vu--)
            {
                int[] level = triStruct[vu];
                int[] above = triStruct[vu - 1];
                for (int va = 1; va < level.Length - 1; va++)
                {
                    int vert = level[va];
                    newPolys.Add(new Polygon(above[va - 1], vert, above[va]));
                }
            }

        }

        polygons = newPolys;
    }

    private static int GetVertFromVec3(List<Vector3> vertices, Vector3 point)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            if (vertices[i].Equals(point)) return i;
        }
        vertices.Add(point);
        return vertices.Count - 1;
    }

    private static void PushVerticesToSphere(List<Vector3> vertices)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] = vertices[i].normalized;
        }
    }

    public struct Polygon
    {
        public int[] vertices;

        public Polygon(int a, int b, int c)
        {
            vertices = new int[] { a, b, c };
        }
    }
}
