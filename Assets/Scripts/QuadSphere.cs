using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadSphere : MonoBehaviour {

    [Range(2,256)]
    public int resolucion = 16; // Resolucion de la esfera.

    [SerializeField,HideInInspector]
     MeshFilter[] meshFilters; // Array con las mallas de cada cara de la esfera.
     QuadFace[] carasEsfera;

    private void OnValidate()
    {
        Initialized();
        GenerateMesh();
    }

    void Initialized()
    {   

        if(meshFilters == null || meshFilters.Length == 0)
            meshFilters = new MeshFilter[6];

        carasEsfera = new QuadFace[6];

        Vector3[] direcciones = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if(meshFilters[i] == null)
            {
            GameObject cara = new GameObject("cara"+(i+1));
            cara.transform.parent = transform;
            cara.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
            meshFilters[i] = cara.AddComponent<MeshFilter>();
            meshFilters[i].sharedMesh = new Mesh();
            }

            carasEsfera[i] = new QuadFace(meshFilters[i].sharedMesh, resolucion, direcciones[i]);

        }
    }

    void GenerateMesh()
    {
        foreach (QuadFace cara in carasEsfera)
        {   
            cara.GenerateMesh();
        }    
    }
    
}
