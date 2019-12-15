using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Referencia: https://lindenreid.wordpress.com/2017/11/07/procedural-sphere-ellipsoid-tutorial/
/// </summary>
public class UVSphere : MonoBehaviour {


    private const float PI = Mathf.PI;
    [Range(min: 1, max: 256)]
    public float width = 8; // width de la esfera (X)
    [Range(min: 1, max: 256)]
    public float length = 8; // length de la esfera (Z)
    [Range(min: 1, max: 256)]
    public float high = 8; // high de la esfera (Y)

    public bool showVertices;

   [Range(min:2,max:256)]
    public int resolution = 32; // Es el numero de subdivisiones de la esfera.


    private List<Vector3> vertices;
    private List<int> triangles;

    private Mesh mesh;
    private bool inicialized = false;

    // Use this for initialization
    void Initialize () {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        mesh = gameObject.AddComponent<MeshFilter>().sharedMesh = new Mesh();
        gameObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
        showVertices = false;
        inicialized = true;
    }



    public void OnValidate()
    {
        if (!inicialized)
            Initialize();
        else
            mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;

        CreateMesh();
        UpdateMesh();
    }

    /// <summary>
    /// Actualiza la mesh de la esfera.
    /// </summary>
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    public void CreateMesh()
    {
        vertices.Clear();
        triangles.Clear();

        GenerateVertex();
        GenerateTris();
    }

    /// <summary>
    /// Crea todos los vertices de la esfera en funcion de los parametros dados.
    /// </summary>
    private void GenerateVertex()
    {

        float theta = PI / resolution; // Angulo abertura del eje Y  , entre 2 pares de vertices. ( Vertical )
        // 2* PI es para trazar todos los puntos de un circulo, si pones un numero menor de 2*PI ( no menor que 0 )
        // puedes generar trozos de la esfera.
        float phi = (2 * PI) / resolution; // Angulo abertura del eje Z , entre 2 pares de vertices. ( Horizoal )

        //Limpiamos nuestra coleccion de vetertices
        vertices.Clear();

        vertices.Add(new Vector3(0, high, 0)); // Vertice superior.

        // Con el bucle de fuera vamos cambiando el radio de los anillos en vertical.
        // Empezamos en 1, debido a que ya ponemos el primer vertice.

        for (float stack = 1; stack <= (resolution - 1); stack++)
        {
            float stackRadiusX = Mathf.Sin(theta * stack) * width;
            float stackRadiusZ = Mathf.Sin(theta * stack) * length;

            for (float slice = 0; slice <= (resolution - 1); slice++)
            {
                float x = Mathf.Cos(phi * slice) * stackRadiusX;
                float y = Mathf.Cos(theta * stack) * high;
                float z = Mathf.Sin(phi * slice) * stackRadiusZ;
                Vector3 vAux = new Vector3(x, y, z);
                vertices.Add(vAux);
            }
        }
        vertices.Add(new Vector3(0, -high, 0)); // Vertice inferior.
    }

    /// <summary>
    /// Crea todos los triangles de la esfera.
    /// </summary>
    private void GenerateTris()
    {

        // Un triangulo esta compuesto por 3 vertices.

        // triangles de arriba

        for (int slice = 0; slice <= (resolution - 2); slice++)
        {
            triangles.AddRange(new int[] { 0, slice + 2, slice + 1 });
        }
        // El ultimo triangulo hay que añadirlo por separado.
        triangles.AddRange(new int[] { 0, 1, resolution });
        
        // quads de en medio

        for (int stack = 0; stack <= (resolution - 3); stack++)
        {
            for (int slice = 0; slice <= (resolution - 2); slice++)
            {
                int v1 = 1 + slice + (resolution * stack);
                int v2 = v1 + 1;
                int v3 = 1 + slice + (resolution * (stack + 1));
                int v4 = v3 + 1;
                triangles.AddRange(new int[] { v1, v2, v4 });
                triangles.AddRange(new int[] { v1, v4, v3 });
            }
            // Ultimo quads de cada fila.
            int v11 = resolution * (stack + 1);
            int v22 = 1 + (resolution * stack);
            int v33 = resolution * (stack + 2);
            int v44 = 1 + (resolution * (stack + 1));
            triangles.AddRange(new int[] { v11, v22, v44 });
            triangles.AddRange(new int[] { v11, v44, v33 });
        }

        // triangles del final

        //Conseguimos el ultimo vertice.
        int vUltimo = vertices.Count - 1;

        for(int slice = 0; slice <= (resolution - 2); slice++)
        {
            int v2 = (resolution - 2) * resolution + slice + 1;
            int v3 = (resolution - 2) * resolution + slice + 2;
            triangles.AddRange(new int[] { vUltimo, v2, v3 });
        }
        triangles.AddRange(new int[] { vUltimo, (resolution -1) * resolution, (resolution - 2) * (resolution + 1) });
    }

    //Funcion auxiliar para ver dibujados los vertices generados.
    private void OnDrawGizmos(){

        if (vertices == null || showVertices == false )
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
