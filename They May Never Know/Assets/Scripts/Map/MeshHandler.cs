using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshHandler : MonoBehaviour
{

    public Mesh mesh;
    public Material mat;
    public GridTester gt;
    public int numOfPoints;
    public meshType type;

    public enum meshType
    {
        mainLand,
        frontSide,
        backSide
    }

    // Start is called before the first frame update
    void Start()
    {
        gt = transform.parent.GetComponent<GridTester>();
        mesh = new Mesh();
        switch (type) {
            case meshType.mainLand:
                float angleStep = 360.0f / (float)numOfPoints;
                List<Vector3> vertexList = new List<Vector3>();
                List<int> triangleList = new List<int>();
                Quaternion quaternion = Quaternion.Euler(0.0f, 0.0f, angleStep);
                // Make first triangle.
                vertexList.Add(new Vector3(0.0f, 0.0f, 0.0f));  // 1. Circle center.
                vertexList.Add(new Vector3(0.0f, gt.Expansion_Distance, 0.0f));  // 2. First vertex on circle outline (radius = 0.5f)
                vertexList.Add(quaternion * vertexList[1]);     // 3. First vertex on circle outline rotated by angle)
                                                                // Add triangle indices.
                triangleList.Add(0);
                triangleList.Add(1);
                triangleList.Add(2);
                for (int i = 0; i < numOfPoints - 1; i++)
                {
                    triangleList.Add(0);                      // Index of circle center.
                    triangleList.Add(vertexList.Count - 1);
                    triangleList.Add(vertexList.Count);
                    vertexList.Add(quaternion * vertexList[vertexList.Count - 1]);
                }
                mesh.vertices = vertexList.ToArray();
                mesh.triangles = triangleList.ToArray();
                /*
                    Vector3[] verticies =
                    {
                        new Vector3(-gt.mapWidth * gt.gridNodeWidth - gt.gridNodeWidth, 0),
                        new Vector3(-gt.gridNodeWidth,gt.mapHeight  * gt.gridNodeHeight),
                        new Vector3(gt.mapWidth * gt.gridNodeWidth - gt.gridNodeWidth, 0),
                        new Vector3(-gt.gridNodeWidth,-gt.mapHeight * gt.gridNodeHeight)
                    };
                    mesh.vertices = verticies;
                    mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
                    for(int i = 0; i < mesh.colors.Length; i++)
                    {
                        mesh.colors[i] = new Color(0f,85f,24f);
                    }
                    */
                break;
            case meshType.frontSide:
                /*
                Vector3[] verticies1 =
                {
                        new Vector3(-gt.mapWidth * gt.gridNodeWidth - gt.gridNodeWidth, 0),
                        new Vector3(-gt.gridNodeWidth,-gt.mapHeight  * gt.gridNodeHeight),
                        new Vector3(-gt.gridNodeWidth, 1.5f * -gt.mapHeight * gt.gridNodeHeight),
                        new Vector3(-gt.mapWidth * gt.gridNodeWidth - gt.gridNodeWidth, -gt.mapHeight * gt.gridNodeHeight /2f)
                    };
                mesh.vertices = verticies1;
                mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
                for (int i = 0; i < mesh.colors.Length; i++)
                {
                    mesh.colors[i] = new Color(202f, 129f, 68f);
                }
                */
                break;
            case meshType.backSide:
                /*
                Vector3[] verticies2 =
                {
                        new Vector3(-gt.gridNodeWidth, -gt.mapHeight * gt.gridNodeHeight),
                        new Vector3(-gt.gridNodeWidth, 1.5f * -gt.mapHeight * gt.gridNodeHeight),
                        new Vector3(gt.mapWidth * gt.gridNodeWidth - gt.gridNodeWidth, 0),
                        new Vector3(gt.mapWidth * gt.gridNodeWidth - gt.gridNodeWidth, -gt.mapHeight * gt.gridNodeHeight/2f)
                    };
                mesh.vertices = verticies2;
                mesh.triangles = new int[] { 0, 1, 2, 1, 2, 3 };
                for (int i = 0; i < mesh.colors.Length; i++)
                {
                    mesh.colors[i] = new Color(128f, 91f, 39f);
                }
                */
                break;
        }
        
        
       // GetComponent<MeshHandler>().

        GetComponent<MeshFilter>().mesh = mesh;
    }

}
