using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    [Range(2,256)]
    public int resolution = 16; //Resolucion de la malla.
    public bool autoUpdate = true; //Controla si se autoactuliza los parametros del planeta o no.

    //Indica las caras del planeta a renderizar.( Se utiliza por temas de rendimiento, para poder trabajar de manera mas fluida)
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    [Tooltip("Indica las caras del planeta a renderizar.")]
    public FaceRenderMask faceRenderMask;

    public ShapeSettings ShapeSettings;
    public ColorSettings ColorSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout,colorSettingsFoldout;

    ShapeGenerator shapeGenerator;

    [SerializeField,HideInInspector]
     MeshFilter[] meshFilters; //Array con las mallas de cada cara de la esfera.
     QuadFace[] SphereFaces;


    void Initialize()
    {   
        shapeGenerator = new ShapeGenerator(ShapeSettings);
    
        if(meshFilters == null || meshFilters.Length == 0)
            meshFilters = new MeshFilter[6];

        SphereFaces = new QuadFace[6];

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
            //Creamos la cara del planeta.
            SphereFaces[i] = new QuadFace(shapeGenerator,meshFilters[i].sharedMesh, resolution, direcciones[i]);
            //Indicamos si esa cara del planeta se va a activar o no.
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);

        }
    }

    private void GenerateMesh()
    {
        for (int i = 0; i < SphereFaces.Length; i++)
        {
            //Generamos solo la malla de los terrenos que estan activos.
            if (meshFilters[i].gameObject.activeSelf)
            {
                SphereFaces[i].GenerateMesh();
            }
        }
    }

    private void GenerateColors()
    {
        foreach (MeshFilter mf in meshFilters){
            mf.GetComponent<MeshRenderer>().sharedMaterial.color = ColorSettings.planetColor;
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public void OnColorSettingsUpdated()
    {
        if(autoUpdate)
        {
            Initialize();
            GenerateColors();
        }
    }

    public void OnShapeSettingsUpdated()
    {
        if(autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }    
    
}
