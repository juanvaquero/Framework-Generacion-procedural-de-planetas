using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Referencia: https://lindenreid.wordpress.com/2017/11/07/procedural-sphere-ellipsoid-tutorial/
/// </summary>
public class UVSphere : MonoBehaviour {


    private const float PI = Mathf.PI;
    [Range(min: 1, max: 256)]
    public float ancho ; // Ancho de la esfera (X)
    [Range(min: 1, max: 256)]
    public float largo ; // Largo de la esfera (Z)
    [Range(min: 1, max: 256)]
    public float alto ; // Alto de la esfera (Y)

    public bool mostrarVertices;

   [Range(min:2,max:256)]
    public int resolucion = 8; // Es el numero de subdivisiones de la esfera.


    private List<Vector3> vertices;
    private List<int> triangulos;

    private Mesh malla;
    private bool inicializado = false;

    // Use this for initialization
    void Initialize () {
        malla = new Mesh();
        vertices = new List<Vector3>();
        triangulos = new List<int>();
        GetComponent<MeshFilter>().mesh = malla;
        mostrarVertices = false;
        inicializado = true;
    }



    public void OnValidate()
    {
        if (!inicializado)
        { 
            Initialize();
        }

        CreateMesh();
        UpdateMesh();
    }

    /// <summary>
    /// Actualiza la malla de la esfera.
    /// </summary>
    void UpdateMesh()
    {
        malla.Clear();
        malla.vertices = vertices.ToArray();
        malla.triangles = triangulos.ToArray();
    }

    public void CreateMesh()
    {
        vertices.Clear();
        triangulos.Clear();

        GenerateVertex();
        GenerateTris();
    }

    /// <summary>
    /// Crea todos los vertices de la esfera en funcion de los parametros dados.
    /// </summary>
    private void GenerateVertex()
    {

        float theta = PI / resolucion; // Angulo abertura del eje Y  , entre 2 pares de vertices. ( Vertical )
        // 2* PI es para trazar todos los puntos de un circulo, si pones un numero menor de 2*PI ( no menor que 0 )
        // puedes generar trozos de la esfera.
        float phi = (2 * PI) / resolucion; // Angulo abertura del eje Z , entre 2 pares de vertices. ( Horizoal )

        //Limpiamos nuestra coleccion de vetertices
        vertices.Clear();

        vertices.Add(new Vector3(0, alto, 0)); // Vertice superior.

        // Con el bucle de fuera vamos cambiando el radio de los anillos en vertical.
        // Empezamos en 1, debido a que ya ponemos el primer vertice.

        for (float stack = 1; stack <= (resolucion - 1); stack++)
        {
            float stackRadiusX = Mathf.Sin(theta * stack) * ancho;
            float stackRadiusZ = Mathf.Sin(theta * stack) * largo;

            for (float slice = 0; slice <= (resolucion - 1); slice++)
            {
                float x = Mathf.Cos(phi * slice) * stackRadiusX;
                float y = Mathf.Cos(theta * stack) * alto;
                float z = Mathf.Sin(phi * slice) * stackRadiusZ;
                Vector3 vAux = new Vector3(x, y, z);
                vertices.Add(vAux);
            }
        }
        vertices.Add(new Vector3(0, -alto, 0)); // Vertice inferior.
    }

    /// <summary>
    /// Crea todos los triangulos de la esfera.
    /// </summary>
    private void GenerateTris()
    {

        // Un triangulo esta compuesto por 3 vertices.

        // triangulos de arriba

        for (int slice = 0; slice <= (resolucion - 2); slice++)
        {
            triangulos.AddRange(new int[] { 0, slice + 2, slice + 1 });
        }
        // El ultimo triangulo hay que añadirlo por separado.
        triangulos.AddRange(new int[] { 0, 1, resolucion });
        
        // quads de en medio

        for (int stack = 0; stack <= (resolucion - 3); stack++)
        {
            for (int slice = 0; slice <= (resolucion - 2); slice++)
            {
                int v1 = 1 + slice + (resolucion * stack);
                int v2 = v1 + 1;
                int v3 = 1 + slice + (resolucion * (stack + 1));
                int v4 = v3 + 1;
                triangulos.AddRange(new int[] { v1, v2, v4 });
                triangulos.AddRange(new int[] { v1, v4, v3 });
            }
            // Ultimo quads de cada fila.
            int v11 = resolucion * (stack + 1);
            int v22 = 1 + (resolucion * stack);
            int v33 = resolucion * (stack + 2);
            int v44 = 1 + (resolucion * (stack + 1));
            triangulos.AddRange(new int[] { v11, v22, v44 });
            triangulos.AddRange(new int[] { v11, v44, v33 });
        }

        // triangulos del final

        //Conseguimos el ultimo vertice.
        int vUltimo = vertices.Count - 1;

        for(int slice = 0; slice <= (resolucion - 2); slice++)
        {
            int v2 = (resolucion - 2) * resolucion + slice + 1;
            int v3 = (resolucion - 2) * resolucion + slice + 2;
            triangulos.AddRange(new int[] { vUltimo, v2, v3 });
        }
        triangulos.AddRange(new int[] { vUltimo, (resolucion -1) * resolucion, (resolucion - 2) * (resolucion + 1) });
    }

    //Funcion auxiliar para ver dibujados los vertices generados.
    private void OnDrawGizmos(){

        if (vertices == null || mostrarVertices == false )
        {
            return;
        }

        Gizmos.color = Color.red;

        foreach(Vector3 v in vertices)
        {
            // TransformPoint allow move the gizmos attach to the Gameobject
            Gizmos.DrawSphere(transform.TransformPoint(v), 0.1f);

            // Without this method, the points doesn't move.
            //Gizmos.DrawSphere(v, 0.1f);
        }


    }



}
