using System.Collections;
using System.Collections.Generic;
using UnityEngine;
   
[RequireComponent(typeof(MeshFilter)),RequireComponent(typeof(MeshRenderer))]
public class GeneratePlane : MonoBehaviour {

    Mesh mesh;

    private Vector3[] vertices;
    private int[] triangles;
    [Range(2,1024)]
    public int  resX = 2;
    [Range(2,1024)]
    public int  resZ = 2;


    private void OnValidate() 
    {
        Inicialize();
        UpdateMesh();
    }

    private void Inicialize()
    {

        if(gameObject.GetComponent<MeshFilter>().sharedMesh == null)
        {
            mesh = gameObject.GetComponent<MeshFilter>().sharedMesh = new Mesh();
            mesh.name = "Terrain";
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
        }

        #region vertices   

        vertices = new Vector3[(resX+1)*(resZ+1)];

        for(int i = 0, z = 0; z <= resZ; z++)
        {
            for(int x = 0; x <= resX; x++)
            {
                float y = Mathf.PerlinNoise(x* 0.3f, z*0.3f) * 2f; // De aqui obtenemos por cada valor de x,z un valor de "y" para esa posicion en 
                                                                   // concreto generado por el perlin noise.
                vertices[i] = new Vector3(x,y,z);
                i++;
            }
        }

        #endregion
        #region triangles

        triangles = new int[(resX)*(resZ)*2*3];// 2 triangulos por quad que contienen a su vez 3 vertices cada uno. 7
        for(int z = 0,vert = 0,tris = 0; z < resZ; z ++)
        {
            for(int x = 0; x < resX; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + resX + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + resX + 1;
                triangles[tris + 5] = vert + resX + 2;

                vert++;
                tris += 6;
            } 
            vert++; // para solucionar un problema de la generacion de los triangulos.
        }

        #endregion

    }

    void CreateShape()
    {

    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}