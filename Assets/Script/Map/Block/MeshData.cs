using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MeshData
{
    public List<Vector3> vertices = new List<Vector3>();    //用于存放网格顶点信息
    public List<int> triangles = new List<int>();           //用于存放网格三角形信息
    public List<Vector2> uv = new List<Vector2>();          //用于存放网格UV信息
    public List<Vector3> colVertices = new List<Vector3>(); //用于存放顶点碰撞信息
    public List<int> colTriangles = new List<int>();        //用于存放三角形碰撞信息

    public bool useRenderDataForCol;    //是否使用碰撞

    public MeshData () { }

    public void AddQuadTriangles ()
    {
        //我们一次调用就添加了四个顶点，因此：
        //vertices.Count - 4代表添加的第一个顶点，以此类推
        //就拿顶面来举例，这两个三角形的组合分别是：123,134
        triangles.Add(vertices.Count - 4);  
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);

        if (useRenderDataForCol)
        {
            colTriangles.Add(colVertices.Count - 4);
            colTriangles.Add(colVertices.Count - 3);
            colTriangles.Add(colVertices.Count - 2);
            colTriangles.Add(colVertices.Count - 4);
            colTriangles.Add(colVertices.Count - 2);
            colTriangles.Add(colVertices.Count - 1);
        }
    }

    public void AddVertex (Vector3 vertex)
    {
        vertices.Add(vertex);
        if (useRenderDataForCol)
        {
            colVertices.Add(vertex);
        }
    }

    public void AddTriangle (int tri)
    {
        triangles.Add(tri);
        if (useRenderDataForCol)
        {
            colTriangles.Add(tri - (vertices.Count - colVertices.Count));
        }
    }
}
