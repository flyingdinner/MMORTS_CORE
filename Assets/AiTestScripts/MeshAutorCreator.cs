using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshAutorCreator : MonoBehaviour
{
    public int segments = 36;
    public float radius = 2.0f;
    

    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int sides = segments;
        float r = radius;

        Vector3[] vertices = new Vector3[(sides + 1) * (sides + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[sides * sides * 6];

        float angleStep = 360.0f / sides;

        for (int i = 0; i <= sides; i++)
        {
            for (int j = 0; j <= sides; j++)
            {
                float angle1 = angleStep * i;
                float angle2 = angleStep * j;

                float x = Mathf.Sin(angle1 * Mathf.Deg2Rad) * (r + Mathf.Cos(angle2 * Mathf.Deg2Rad));
                float y = Mathf.Cos(angle1 * Mathf.Deg2Rad) * (r + Mathf.Cos(angle2 * Mathf.Deg2Rad));
                float z = Mathf.Sin(angle2 * Mathf.Deg2Rad);

                int index = i * (sides + 1) + j;
                vertices[index] = new Vector3(x, y, z);
                uv[index] = new Vector2((float)i / sides, (float)j / sides);

                if (i < sides && j < sides)
                {
                    int tIndex = (i * sides + j) * 6;
                    triangles[tIndex] = index;
                    triangles[tIndex + 1] = index + sides + 1;
                    triangles[tIndex + 2] = index + 1;
                    triangles[tIndex + 3] = index + 1;
                    triangles[tIndex + 4] = index + sides + 1;
                    triangles[tIndex + 5] = index + sides + 2;
                }
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        

        InitMesh(mesh);
    }

    private void InitMesh(Mesh mesh) {
        
        mesh.RecalculateNormals(UnityEngine.Rendering.MeshUpdateFlags.DontRecalculateBounds);
        Flip(mesh);
        // Присвоение меша компоненту MeshFilter
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void Flip(Mesh mesh)
    {        
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
            normals[i] = -normals[i];
        mesh.normals = normals;

        for (int m = 0; m < mesh.subMeshCount; m++)
        {
            int[] triangles = mesh.GetTriangles(m);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int temp = triangles[i + 0];
                triangles[i + 0] = triangles[i + 1];
                triangles[i + 1] = temp;
            }
            mesh.SetTriangles(triangles, m);
        }
    }
}
