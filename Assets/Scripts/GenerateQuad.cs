using System.Collections;
using System.Collections.Generic;
using UnityEngine;
   
[RequireComponent(typeof(MeshFilter))]
public class GenerateQuad : MonoBehaviour {

    Mesh malla;

    public Vector3[] vertices;
    public int[] triangulos;


	// Use this for initialization
	void Start () {
        malla = new Mesh();

        GetComponent<MeshFilter>().mesh = malla;

       // CreateShape();
    }

    void Update()
    {

        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,0),
            new Vector3(1,0,1)

        };

        triangulos = new int[]
        {
            0,1,2,  
            2,1,3
        };
    }

    void UpdateMesh()
    {
        malla.Clear();
        malla.vertices = vertices;
        malla.triangles = triangulos;
    }
}
