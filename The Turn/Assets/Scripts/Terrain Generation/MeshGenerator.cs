using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    List<Vector3> vertices;
    List<int> triangles;

    public int xSize;
    public int zSize;
    public float squareSize;
    public float heightLimit;
    public float perlinScale;

    Quad[,] quads;

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.name = "Terrain Mesh";

        vertices = new List<Vector3>();
        triangles = new List<int>();

        // Generate quad array
        quads = new Quad[xSize, zSize];

        // Generate terrain
        int vertCount = 0;
        for(int i = 0; i < xSize; i++)
        {
            for(int j = 0; j < zSize; j++)
            {
                vertCount += CreateQuad(i, j, vertCount);
            }
        }

        UpdateMesh();
    }

    int CreateQuad(int offsetX, int offsetZ, int count)
    {
        
        int vertCount = 0;
        Quad q = new Quad();

        if (offsetX == 0 && offsetZ == 0)
        {
            // Generate Base Quad
            q.vert0 = new Vector3(0, calculateHeight(offsetX, offsetZ), 0);
            q.vert1 = new Vector3(0, calculateHeight(offsetX, offsetZ + 1), 1 * squareSize);
            q.vert2 = new Vector3(1 * squareSize, calculateHeight(offsetX + 1, offsetZ), 0);
            q.vert3 = new Vector3(1 * squareSize, calculateHeight(offsetX + 1, offsetZ + 1), 1 * squareSize);

            q.index0 = 0;
            q.index1 = 1;
            q.index2 = 2;
            q.index3 = 1;
            q.index4 = 3;
            q.index5 = 2;

            vertCount = 4;
        }
        else if (offsetX == 0)
        {
            // generate Z linked quad
            Quad zQuad = quads[offsetX, offsetZ - 1];

            q.vert0 = zQuad.vert1;
            q.vert1 = new Vector3((offsetX + 0) * squareSize, calculateHeight(offsetX, offsetZ + 1), (offsetZ + 1) * squareSize);
            q.vert2 = zQuad.vert3;
            q.vert3 = new Vector3((offsetX + 1) * squareSize, calculateHeight(offsetX + 1, offsetZ + 1), (offsetZ + 1) * squareSize);

            q.index0 = 0 + count;
            q.index1 = 1 + count;
            q.index2 = 2 + count;
            q.index3 = 1 + count;
            q.index4 = 3 + count;
            q.index5 = 2 + count;

            vertCount = 4;

        }
        else if (offsetZ == 0)
        {
            // Generate X linked quad
            Quad xQuad = quads[offsetX - 1, offsetZ];

            q.vert0 = xQuad.vert2;
            q.vert1 = xQuad.vert3;
            q.vert2 = new Vector3((offsetX + 1) * squareSize, calculateHeight(offsetX + 1, offsetZ + 0), (offsetZ + 0) * squareSize);
            q.vert3 = new Vector3((offsetX + 1) * squareSize, calculateHeight(offsetX + 1, offsetZ + 1), (offsetZ + 1) * squareSize);

            q.index0 = 0 + count;
            q.index1 = 1 + count;
            q.index2 = 2 + count;
            q.index3 = 1 + count;
            q.index4 = 3 + count;
            q.index5 = 2 + count;

            vertCount = 4;
        }
        else
        {
            // Generate X and Z linked Quad
            Quad xzQuad = quads[offsetX - 1, offsetZ - 1];
            Quad xQuad = quads[offsetX - 1, offsetZ];
            Quad zQuad = quads[offsetX, offsetZ - 1];

            q.vert0 = xzQuad.vert3;
            q.vert1 = xQuad.vert3;
            q.vert2 = zQuad.vert3;
            q.vert3 = new Vector3((offsetX + 1) * squareSize, calculateHeight(offsetX + 1, offsetZ + 1), (offsetZ + 1) * squareSize);

            q.index0 = 0 + count;
            q.index1 = 1 + count;
            q.index2 = 2 + count;
            q.index3 = 1 + count;
            q.index4 = 3 + count;
            q.index5 = 2 + count;

            vertCount = 4;
        }

        quads[offsetX, offsetZ] = q;

        Vector3[] verticesToAdd = new Vector3[]
            {
                q.vert0,
                q.vert1,
                q.vert2,
                q.vert3
            };

        int[] trianglesToAdd = new int[]
        {
                q.index0,
                q.index1,
                q.index2,
                q.index3,
                q.index4,
                q.index5
        };

        AddVertices(verticesToAdd);
        AddTriangles(trianglesToAdd);

        return vertCount;
    }

    float calculateHeight(int x, int z)
    {
        float xCoord = (float) x / xSize * perlinScale;
        float zCoord = (float) z / zSize * perlinScale;

        return Mathf.PerlinNoise(xCoord, zCoord) * heightLimit;
    }

    void AddVertices(Vector3[] toAdd)
    {
        for (int i = 0; i < toAdd.Length; i++)
        {
            vertices.Add(toAdd[i]);
        }
    }

    void AddTriangles(int[] toAdd)
    {
        for (int i = 0; i < toAdd.Length; i++)
        {
            triangles.Add(toAdd[i]);
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public Quad[,] GetQuads()
    {
        return quads;
    }
}
