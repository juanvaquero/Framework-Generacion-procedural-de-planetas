using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    [Range(2,256)]
    public int resolution = 16; //Resolucion de la malla.
    public bool autoUpdate = true; //Controla si se autoactuliza los parametros del planeta o no.

    public bool randomGeneration = false; //Controla si se quiere generar o no un planeta poniendo los parametros de manera aletoria.

    //Indica las caras del planeta a renderizar.( Se utiliza por temas de rendimiento, para poder trabajar de manera mas fluida)
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    [Tooltip("Indica las caras del planeta a renderizar.")]
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;

    [HideInInspector]
    public bool shapeSettingsFoldout,colorSettingsFoldout;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColorGenerator colorGenerator = new ColorGenerator();

    [SerializeField,HideInInspector]
     MeshFilter[] meshFilters; //Array con las mallas de cada cara de la esfera.
     QuadFace[] SphereFaces;

    //Para que lo genere cuando entras en playMode.
    private void Start() {
        GeneratePlanet();
    }
    //Para que se actualizase al salir del playMode.
        private void OnValidate() {
            GeneratePlanet();
        }

    void Initialize()
    {   
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);
 
    
        if(meshFilters == null || meshFilters.Length == 0)
            meshFilters = new MeshFilter[6];

        SphereFaces = new QuadFace[6];

        Vector3[] direcciones = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if(meshFilters[i] == null)
            {
            GameObject cara = new GameObject("cara"+(i+1)+": "+direcciones[i]);
            cara.transform.parent = transform;
            cara.AddComponent<MeshRenderer>(); 
            meshFilters[i] = cara.AddComponent<MeshFilter>();
            meshFilters[i].sharedMesh = new Mesh();
            }
            // Asignamos el material a cada cara del planeta.
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.material;
            //Creamos la cara del planeta.
            SphereFaces[i] = new QuadFace(shapeGenerator,colorGenerator,meshFilters[i].sharedMesh, resolution, direcciones[i]);
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
                SphereFaces[i].GenerateMesh();
        }
        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    private void GenerateColors()
    {
        colorGenerator.UpdateColors();

        for (int i = 0; i < SphereFaces.Length; i++)
        {
            //Generamos solo la malla de los terrenos que estan activos.
            if (meshFilters[i].gameObject.activeSelf)
                SphereFaces[i].UpdateUvs();
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
