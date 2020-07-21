using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadFace {

    private int resolution; // indica la cantidad de triangunlos de cada cara del cubo.
    private Mesh mesh;
    private Vector3 localUp;
    private Vector3 ejeA;
    private Vector3 ejeB;
    private ShapeGenerator shapeGenerator;
    private ColorGenerator colorGenerator;

    //TODO Reahacer la documentacion de estas funciones.
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="resolution"></param>
    /// <param name="localUp">Vector que indica la direccion hacia donde esta orientada la cara.</param>
    public QuadFace(ShapeGenerator shapeGenerator,ColorGenerator colorGenerator,Mesh mesh, int resolution, Vector3 localUp)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        this.ejeA = new Vector3(localUp.y, localUp.z, localUp.x);
        // hacemos el producto del ejeA con el eje de direccion para conseguir el ejeB.
        this.ejeB = Vector3.Cross(localUp, ejeA); 

        this.shapeGenerator = shapeGenerator;
        this.colorGenerator = colorGenerator;
    }

	public void GenerateMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        List<int> triangulos = new List<int>();
        //Guardamos los uvs de la malla para no perderlos cuando la volvamos a generar.
        // Vector2[] uvs = mesh.uv;
        Vector2[] uvs;
        //Comprobamos que no se ha cambiado la resolucion del planeta,
        //si esta ha cambiado reajustamos el tamaño de nuestra uvs.
        if( mesh.uv.Length == vertices.Length)
        {
            uvs = mesh.uv;
        }
        else
        {
            uvs = new Vector2[vertices.Length];
        }

        

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                
                Vector2 porcentaje = new Vector2(x, y) / (resolution -1);
                Vector3 puntoActualCubo = localUp + (porcentaje.x -0.5f) * 2 * ejeA
                    + (porcentaje.y -0.5f) * 2 * ejeB;
                Vector3 puntoActualEsfera = puntoActualCubo.normalized;
                //Revisar
                // vertices[i] = shapeGenerator.CalculatePointOnQuadSphere(puntoActualEsfera);  
                float unScaledElevation = shapeGenerator.CalculateUnscaledElevation(puntoActualEsfera);
                vertices[i] = puntoActualEsfera * shapeGenerator.GetScaledElevation(unScaledElevation);
                //Nos guardamos el valor de la elevacion del planeta sin escalarla en la Y , 
                //para asi poder pintar la profundidad de nuestro planeta.
                uvs[i].y = unScaledElevation;

                //TODO mirar luego si cambiarlo a menor que resolution.
                if ( x != (resolution-1) && y != (resolution-1))
                {
                    int v1 = i;
                    int v2 = i + resolution + 1;
                    int v3 = i + resolution;
                    triangulos.AddRange(new int[] { v1, v2, v3 });
                    int v4 = i + 1;
                    triangulos.AddRange(new int[] { v1, v4, v2 });
                }

            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangulos.ToArray();
        mesh.RecalculateNormals();
        if(mesh.uv.Length == uvs.Length)
            mesh.uv = uvs;
    }

    public void UpdateUvs()
    {
        // Vector2[] uvs = new Vector2[resolution*resolution];
        Vector2[] uvs = mesh.uv;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                
                Vector2 porcentaje = new Vector2(x, y) / (resolution -1);
                Vector3 puntoActualCubo = localUp + (porcentaje.x -0.5f) * 2 * ejeA
                    + (porcentaje.y -0.5f) * 2 * ejeB;
                Vector3 puntoActualEsfera = puntoActualCubo.normalized;

                // uvs[i] = new Vector2(colorGenerator.BiomePercentFromPoint(puntoActualEsfera),0);
                //En la X almaceamos el porcentaje de bioma del punto actual.
                uvs[i].x = colorGenerator.BiomePercentFromPoint(puntoActualEsfera);
               
            }
        }
        mesh.uv = uvs;
    }
}
