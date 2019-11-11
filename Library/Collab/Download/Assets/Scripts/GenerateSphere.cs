using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSphere : MonoBehaviour {


    private const float PI = Mathf.PI;

    public float width; // Ancho de la esfera (X)
    public float length; // Largo de la esfera (Z)
    public float heigth; // Alto de la esfera (Y)

    public float Slices; // Es el numero de subdivisiones.

    //private Vector3[] vertices;
    private HashSet<Vector3> vertices;
    private int[] triangulos;

    // Use this for initialization
    void Start () {
        vertices = new HashSet<Vector3>();
    }

    private void Update()
    {
        CreateMesh();
    }

    
    /// <summary>
    /// Crea todos los vertices de la esfera en funcion de los parametros dados.
    /// </summary>
    public void CreateMesh()
    {

        float theta = PI / Slices; // Angulo abertura del eje Y  , entre 2 pares de vertices. ( Vertical )
        // 2* PI es para trazar todos los puntos de un circulo, si pones un numero menor de 2*PI ( no menor que 0 )
        // puedes generar trozos de la esfera.
        float phi = (2 * PI) / Slices; // Angulo abertura del eje Z , entre 2 pares de vertices. ( Horizoal )

        //Limpiamos nuestra coleccion de vetertices
        vertices.Clear();

        vertices.Add(new Vector3(0, heigth, 0)); // Vertice superior.

        // Con el bucle de fuera vamos cambiando el radio de los anillos en vertical.
        // Empezamos en 1, debido a que ya ponemos el primer vertice.

        for (float stack = 1; stack <= (Slices - 1); stack++)
        {
            float stackRadiusX = Mathf.Sin(theta * stack) * width;
            float stackRadiusZ = Mathf.Sin(theta * stack) * length;

            for (float slice = 0; slice <= (Slices - 1); slice++)
            {
                float x = Mathf.Cos(phi * slice) * stackRadiusX;
                float y = Mathf.Cos(theta * stack) * heigth;
                float z = Mathf.Sin(phi * slice) * stackRadiusZ;
                Vector3 vAux = new Vector3(x, y, z);
                vertices.Add(vAux);
            }
        }
        vertices.Add(new Vector3(0, -heigth, 0)); // Vertice inferior.
    }

    /// <summary>
    /// Crea todos los triangulos de la esfera.
    /// </summary>
    private void GenerateTris()
    {
        
    }




    //Funcion auxiliar para ver dibujados los vertices generados.
    private void OnDrawGizmos(){

        if (vertices == null)
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
