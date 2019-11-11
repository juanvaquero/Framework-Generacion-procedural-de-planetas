using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadFace {

    private int resolucion; // indica la cantidad de triangunlos de cada cara del cubo.
    private Mesh malla;
    private Vector3 localUp;
    private Vector3 ejeA;
    private Vector3 ejeB;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="malla"></param>
    /// <param name="resolucion"></param>
    /// <param name="localUp">Vector que indica la direccion hacia donde esta orientada la cara.</param>
    public QuadFace(Mesh malla, int resolucion, Vector3 localUp)
    {
        this.malla = malla;
        this.resolucion = resolucion;
        this.localUp = localUp;

        this.ejeA = new Vector3(localUp.y, localUp.z, localUp.x);
        // hacemos el producto del ejeA con el eje de direccion para conseguir el ejeB.
        this.ejeB = Vector3.Cross(localUp, ejeA); 
    }

	public void GenerateMesh()
    {
        Vector3[] vertices = new Vector3[resolucion * resolucion];
        List<int> triangulos = new List<int>();

        for (int y = 0; y < resolucion; y++)
        {
            for (int x = 0; x < resolucion; x++)
            {
                int i = x + y * resolucion;

                Vector2 porcentaje = new Vector2(x, y) / (resolucion -1);
                Vector3 puntoActualCubo =localUp + (porcentaje.x -0.5f) * 2 * ejeA
                    + (porcentaje.y -0.5f) * 2 * ejeB;
                Vector3 puntoActualEsfera = puntoActualCubo.normalized;

                vertices[i] = puntoActualEsfera;  

                // mirar luego si cambiarlo a menor que resolucion.
                if ( x != (resolucion-1) && y != (resolucion-1))
                {
                    int v1 = i;
                    int v2 = i + resolucion + 1;
                    int v3 = i + resolucion;
                    triangulos.AddRange(new int[] { v1, v2, v3 });
                    int v4 = i + 1;
                    triangulos.AddRange(new int[] { v1, v4, v2 });
                }

            }
        }

        malla.Clear();
        malla.vertices = vertices;
        malla.triangles = triangulos.ToArray();
        malla.RecalculateNormals();
    }


}
